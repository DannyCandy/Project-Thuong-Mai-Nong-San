document.addEventListener("DOMContentLoaded", function () {
    fetch('/api/Statistics/GetVisitorData')
        .then(response => response.json())
        .then(data => {
            const labels = data.map(d => d.date);
            const values = data.map(d => d.count);

            // Hiển thị số lượt truy cập hôm nay
            const todayCount = values[values.length - 1] || 0;
            document.getElementById('visitorCount').textContent = `Hôm nay: ${todayCount} lượt`;

            // Tạo biểu đồ
            const ctx = document.getElementById('visitorChart').getContext('2d');
            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Lượt truy cập',
                        data: values,
                        borderColor: 'rgba(75, 192, 192, 1)',
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        borderWidth: 2,
                        pointRadius: 3,
                        tension: 0.3,
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        x: {
                            ticks: {
                                maxRotation: 50,
                                minRotation: 30,
                                autoSkip: true,
                            },
                            grid: {
                                color: '#eaeaea',
                                borderDash: [5, 5],
                            },
                        },
                        y: {
                            beginAtZero: true,
                            suggestedMax: Math.max(...values) + 10,
                            grid: {
                                color: '#eaeaea',
                                borderDash: [5, 5],
                            },
                        }
                    },
                    plugins: {
                        tooltip: {
                            mode: 'index',
                            intersect: false,
                            backgroundColor: '#333',
                            titleColor: '#fff',
                            bodyColor: '#fff',
                        },
                        legend: {
                            display: false,
                        }
                    }
                }
            });
        })
        .catch(error => {
            console.error('Lỗi khi lấy dữ liệu:', error);
        });
});
