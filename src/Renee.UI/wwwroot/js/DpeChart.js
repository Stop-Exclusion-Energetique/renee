window.initializeChart = (canvasId, chartType, labels, data, backgroundColors) => {
    const ctx = document.getElementById(canvasId).getContext('2d');

    // Plugin pour afficher les données devant chaque barre
    const dataLabelPlugin = {
        id: 'dataLabel',
        afterDatasetsDraw: chart => {
            const ctx = chart.ctx;
            const dataset = chart.data.datasets[0];
            const meta = chart.getDatasetMeta(0);

            ctx.save();
            ctx.font = '12px Arial';
            ctx.fillStyle = 'black';

            meta.data.forEach((bar, index) => {
                const value = dataset.data[index];
                const {x, y} = bar.tooltipPosition();

                // Afficher la donnée légèrement décalée à droite de la flèche
                ctx.fillText(value, x + 19.50, y + 4);
            });

            ctx.restore();
        }
    };

    // Plugin pour dessiner des barres sous forme de flèches
    const arrowBarPlugin = {
        id: 'arrowBar',
        beforeDraw: chart => {
            const ctx = chart.ctx;
            const dataset = chart.data.datasets[0];
            const meta = chart.getDatasetMeta(0);

            meta.data.forEach((bar, index) => {
                const {x, y} = bar.tooltipPosition();

                ctx.save();
                ctx.fillStyle = dataset.backgroundColor[index];

                // Taille de la flèche
                const arrowHeight = 9.6;
                const arrowWidth = 20;

                // Position de la flèche (extrémité droite)
                const arrowX = x + 19.25; // Position x de la pointe de la flèche
                const arrowY = y;

                // Dessiner la flèche
                ctx.beginPath();
                ctx.moveTo(arrowX, arrowY); // Pointe de la flèche
                ctx.lineTo(arrowX - arrowWidth, arrowY - arrowHeight); // Coin supérieur gauche
                ctx.lineTo(arrowX - arrowWidth, arrowY + arrowHeight); // Coin inférieur gauche
                ctx.closePath();
                ctx.fill();

                ctx.restore();
            });
        }
    };

    // Ajouter le plugin lors de la création du graphe
    Chart.register(dataLabelPlugin, arrowBarPlugin);

    try {
        new Chart(ctx, {
            type: chartType, // Utilisez "bar" pour un graphe horizontal
            data: {
                labels,
                datasets: [{
                    label: '',
                    data,
                    backgroundColor: backgroundColors,
                    borderWidth: 0
                }]
            },
            options: {
                indexAxis: 'y', // Graphe horizontal
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {display: false}
                },
                scales: {
                    x: {
                        display: false,
                        beginAtZero: true,
                        suggestedMax: Math.max(...data) * 1.2,
                        grid: {display: false}
                    },
                    y: {
                        grid: {display: false}
                    }
                }
            }
        });
    } catch (error) {
        console.error(error);
    }
};
