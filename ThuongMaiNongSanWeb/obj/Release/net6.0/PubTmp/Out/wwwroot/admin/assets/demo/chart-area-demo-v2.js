// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

var dataDaiLyJsArea = [
    {
        "year": 2024,
        "month": 1,
        "totalPrice": 0
    },
    {
        "year": 2024,
        "month": 2,
        "totalPrice": 0
    },
    {
        "year": 2024,
        "month": 3,
        "totalPrice": 0
    },
    {
        "year": 2024,
        "month": 4,
        "totalPrice": 0
    },
    {
        "year": 2024,
        "month": 5,
        "totalPrice": 0
    },
    {
        "year": 2024,
        "month": 6,
        "totalPrice": 0
    },
    {
        "year": 2024,
        "month": 7,
        "totalPrice": 0
    },
    {
        "year": 2024,
        "month": 8,
        "totalPrice": 0
    },
    {
        "year": 2024,
        "month": 9,
        "totalPrice": 0
    },
    {
        "year": 2024,
        "month": 10,
        "totalPrice": 0
    },
    {
        "year": 2024,
        "month": 11,
        "totalPrice": 0
    },
    {
        "year": 2024,
        "month": 12,
        "totalPrice": 0
    }
];
let maxTotalPriceV2 = 0;
//Làm tròn giá trị lớn nhất
function roundUpToNearestPowerOfTenV2(number) {
    // Tìm số chữ số của number (không tính phần thập phân)
    const numberOfDigits = Math.floor(Math.log10(number)) + 1;

    // Tính giá trị làm tròn
    const powerOfTen = Math.pow(10, numberOfDigits - 1);
    const roundedNumber = Math.ceil(number / powerOfTen) * powerOfTen;

    return roundedNumber;
}
//Fetch data from DB

async function fetchDataDaiLyArea() {
    try {
        const idDaiLy = localStorage.getItem("IdDaiLyCurrent")
        console.log("From Manager: " + idDaiLy)
        const response = await fetch('/api/productsDaiLy/total-price/2024/' + idDaiLy, { method: 'GET' });
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const jsonData = await response.json();
        return jsonData;
        // Xử lý dữ liệu trả về ở đây
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
    }
}
// Định nghĩa hàm init để khởi tạo biểu đồ
async function initChartAreaDaiLy() {
    try {
        const dataFetched = await fetchDataDaiLyArea();
        if (typeof dataFetched === 'object' && dataFetched.length === 0) {
            //Do not thing
        } else {
            dataFetched.forEach(function (newItem) {
                // Tìm index của phần tử có month tương ứng trong dataDaiLyJsArea
                var indexToUpdate = dataDaiLyJsArea.findIndex(item => item.month === newItem.month);

                // Nếu tìm thấy index, cập nhật dữ liệu của tháng đó
                if (indexToUpdate !== -1) {
                    dataDaiLyJsArea[indexToUpdate] = newItem;
                }

                // Cập nhật maxTotalPrice nếu totalPrice của newItem lớn hơn maxTotalPrice hiện tại
                if (newItem.totalPrice > maxTotalPriceV2) {
                    maxTotalPriceV2 = newItem.totalPrice;
                }
            });
        }
        // Xử lý dữ liệu và vẽ biểu đồ ở đây
        drawAreaChartDaiLy();
    } catch (error) {
        console.error('There was a problem initializing the chart:', error);
    }
}

// Gọi hàm init khi tài liệu đã được tải
document.addEventListener('DOMContentLoaded', initChartAreaDaiLy);

// Area Chart Example
function drawAreaChartDaiLy() {
    var ctx = document.getElementById("myAreaChart");
    var myLineChartDaiLy = new Chart(ctx, {
        type: "line",
        data: {
            labels: ["Jan", "Feb", "Mar", "April", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec"],
            datasets: [{
                label: "TotalPrice",
                lineTension: 0.3,
                backgroundColor: "rgba(2,117,216,0.2)",
                borderColor: "rgba(2,117,216,1)",
                pointRadius: 5,
                pointBackgroundColor: "rgba(2,117,216,1)",
                pointBorderColor: "rgba(255,255,255,0.8)",
                pointHoverRadius: 5,
                pointHoverBackgroundColor: "rgba(2,117,216,1)",
                pointHitRadius: 50,
                pointBorderWidth: 2,
                data: [dataDaiLyJsArea[0].totalPrice, dataDaiLyJsArea[1].totalPrice,
                dataDaiLyJsArea[2].totalPrice, dataDaiLyJsArea[3].totalPrice,
                dataDaiLyJsArea[4].totalPrice, dataDaiLyJsArea[5].totalPrice,
                dataDaiLyJsArea[6].totalPrice, dataDaiLyJsArea[7].totalPrice,
                dataDaiLyJsArea[8].totalPrice, dataDaiLyJsArea[9].totalPrice,
                dataDaiLyJsArea[10].totalPrice, dataDaiLyJsArea[11].totalPrice],
            }],
        },
        options: {
            scales: {
                xAxes: [{
                    time: {
                        unit: 'month'
                    },
                    gridLines: {
                        display: false
                    },
                    ticks: {
                        maxTicksLimit: 7
                    }
                }],
                yAxes: [{
                    ticks: {
                        min: 0,
                        max: roundUpToNearestPowerOfTenV2(maxTotalPriceV2),
                        maxTicksLimit: 11
                    },
                    gridLines: {
                        color: "rgba(0, 0, 0, .125)",
                    }
                }],
            },
            legend: {
                display: false
            }
        }
    });
}

