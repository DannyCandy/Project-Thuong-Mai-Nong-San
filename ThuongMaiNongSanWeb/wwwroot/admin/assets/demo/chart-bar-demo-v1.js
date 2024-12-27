// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

var dataJsBar = [
    {
        "year": 2024,
        "month": 1,
        "totalQuantity": 0
    },
    {
        "year": 2024,
        "month": 2,
        "totalQuantity": 0
    },
    {
        "year": 2024,
        "month": 3,
        "totalQuantity": 0
    },
    {
        "year": 2024,
        "month": 4,
        "totalQuantity": 0
    },
    {
        "year": 2024,
        "month": 5,
        "totalQuantity": 0
    },
    {
        "year": 2024,
        "month": 6,
        "totalQuantity": 0
    },
    {
        "year": 2024,
        "month": 7,
        "totalQuantity": 0
    },
    {
        "year": 2024,
        "month": 8,
        "totalQuantity": 0
    },
    {
        "year": 2024,
        "month": 9,
        "totalQuantity": 0
    },
    {
        "year": 2024,
        "month": 10,
        "totalQuantity": 0
    },
    {
        "year": 2024,
        "month": 11,
        "totalQuantity": 0
    },
    {
        "year": 2024,
        "month": 12,
        "totalQuantity": 0
    }
];
let maxTotalQuantityV1 = 0;
function roundUpToNearestPowerOfTenQttV1(number) {
    // Tìm số chữ số của number (không tính phần thập phân)
    const numberOfDigits = Math.floor(Math.log10(number)) + 1;

    // Tính giá trị làm tròn
    const powerOfTen = Math.pow(10, numberOfDigits - 1);
    const roundedNumber = Math.ceil(number / powerOfTen) * powerOfTen;

    return roundedNumber;
}
//Fetch data from DB
async function fetchDataBar() {
    try {
        const response = await fetch('/api/products/total-quantity/2024', { method: 'GET' });
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
async function initChartBar() {
    try {
        const dataFetched = await fetchDataBar();
        if (typeof dataFetched === 'object' && dataFetched.length === 0) {
            //Do not thing
        } else {
            dataFetched.forEach(function (newItem) {
                // Tìm index của phần tử có month tương ứng trong dataDaiLyJsArea
                var indexToUpdate = dataJsBar.findIndex(item => item.month === newItem.month);
                
                // Nếu tìm thấy index, cập nhật dữ liệu của tháng đó
                if (indexToUpdate !== -1) {
                    dataJsBar[indexToUpdate] = newItem;
                }

                if (newItem.totalQuantity > maxTotalQuantityV1) {
                    maxTotalQuantityV1 = newItem.totalQuantity;
                }
            });
        }
        // Xử lý dữ liệu và vẽ biểu đồ ở đây
        drawBarChart();
    } catch (error) {
        console.error('There was a problem initializing the chart:', error);
    }
}

// Gọi hàm init khi tài liệu đã được tải
document.addEventListener('DOMContentLoaded', initChartBar);

// Bar Chart Example
function drawBarChart() {
    var ctx = document.getElementById("myBarChart");
    var myBarChart = new Chart(ctx, {
        type: "bar",
        data: {
            labels: ["Jan", "Feb", "Mar", "April", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec"],
            datasets: [{
                label: "Quantity",
                backgroundColor: "rgba(2,117,216,1)",
                borderColor: "rgba(2,117,216,1)",
                data: [dataJsBar[0].totalQuantity, dataJsBar[1].totalQuantity,
                dataJsBar[2].totalQuantity, dataJsBar[3].totalQuantity,
                dataJsBar[4].totalQuantity, dataJsBar[5].totalQuantity,
                dataJsBar[6].totalQuantity, dataJsBar[7].totalQuantity,
                dataJsBar[8].totalQuantity, dataJsBar[9].totalQuantity,
                dataJsBar[10].totalQuantity, dataJsBar[11].totalQuantity],
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
                        maxTicksLimit: 12
                    }
                }],
                yAxes: [{
                    ticks: {
                        min: 0,
                        max: roundUpToNearestPowerOfTenQttV1(maxTotalQuantityV1),
                        maxTicksLimit: 5
                    },
                    gridLines: {
                        display: true
                    }
                }],
            },
            legend: {
                display: false
            }
        }
    });
}

