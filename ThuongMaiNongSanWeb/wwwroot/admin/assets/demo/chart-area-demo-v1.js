// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

var dataJsArea = [
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
let maxTotalPriceV1 = 0;
//Làm tròn giá trị lớn nhất
function roundUpToNearestPowerOfTenV1(number) {
    // Tìm số chữ số của number (không tính phần thập phân)
    const numberOfDigits = Math.floor(Math.log10(number)) + 1;

    // Tính giá trị làm tròn
    const powerOfTen = Math.pow(10, numberOfDigits - 1);
    const roundedNumber = Math.ceil(number / powerOfTen) * powerOfTen;

    return roundedNumber;
}
//Fetch data from DB
async function fetchDataArea() {
    try {
        const response = await fetch('/api/products/total-price/2024', { method: 'GET' });
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
async function initChartArea() {
    try {
        const dataFetched = await fetchDataArea();
        if (typeof dataFetched === 'object' && dataFetched.length === 0) {
            //Do not thing
        } else {
            //Xử lý dữ liệu mới
            dataFetched.forEach(function (newItem) {
                // Tìm index của phần tử có month tương ứng trong dataDaiLyJsArea
                var indexToUpdate = dataJsArea.findIndex(item => item.month === newItem.month);

                // Nếu tìm thấy index, cập nhật dữ liệu của tháng đó
                if (indexToUpdate !== -1) {
                    dataJsArea[indexToUpdate] = newItem;
                } 

                // Cập nhật maxTotalPrice nếu totalPrice của newItem lớn hơn maxTotalPrice hiện tại
                if (newItem.totalPrice > maxTotalPriceV1) {
                    maxTotalPriceV1 = newItem.totalPrice;
                }
            });
        }
        // Xử lý dữ liệu và vẽ biểu đồ ở đây
        drawAreaChart();
    } catch (error) {
        console.error('There was a problem initializing the chart:', error);
    }
}

// Gọi hàm init khi tài liệu đã được tải
document.addEventListener('DOMContentLoaded', initChartArea);

// Area Chart Example
function drawAreaChart() {
    var ctx = document.getElementById("myAreaChart");
    var myLineChart = new Chart(ctx, {
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
                data: [dataJsArea[0].totalPrice, dataJsArea[1].totalPrice,
                dataJsArea[2].totalPrice, dataJsArea[3].totalPrice,
                dataJsArea[4].totalPrice, dataJsArea[5].totalPrice,
                dataJsArea[6].totalPrice, dataJsArea[7].totalPrice,
                dataJsArea[8].totalPrice, dataJsArea[9].totalPrice,
                dataJsArea[10].totalPrice, dataJsArea[11].totalPrice],
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
                        max: roundUpToNearestPowerOfTenV1(maxTotalPriceV1),
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

