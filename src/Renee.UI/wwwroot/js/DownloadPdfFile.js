window.triggerInputFile = function (inputFile) {
    inputFile.click();
};

window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? 'download';
    anchorElement.click();
    URL.revokeObjectURL(url);
};

window.downloadFile = function (fileName, byteBase64) {
    let link = document.createElement('a');
    link.download = fileName;
    link.href = "data:application/pdf;base64," + byteBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

window.viewFile = function (base64Data, mimeType, fileName)
{
    const byteCharacters = atob(base64Data);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++)
    {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: mimeType });

    const url = URL.createObjectURL(blob);

    const visualizableMimeTypes = ["application/pdf", "image/jpeg", "image/png", "image/webp"];

    if (visualizableMimeTypes.includes(mimeType))
    {
        const newTab = window.open();
        newTab.location.href = url;
    }
    else
    {
        const link = document.createElement("a");
        link.href = url;
        link.download = fileName;
        link.click();
    }

    // Nettoyer l'URL Blob apr�s usage
    URL.revokeObjectURL(url);
}
window.pdfViewer = {
    pdfDoc: null,
    currentPage: 1,
    totalPages: 1,
    canvasId: null,

    loadPdf(url, canvasId) {
        this.canvasId = canvasId;
        pdfjsLib.GlobalWorkerOptions.workerSrc = "https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.16.105/pdf.worker.min.js";

        pdfjsLib.getDocument(url).promise.then(pdf => {
            this.pdfDoc = pdf;
            this.totalPages = pdf.numPages;
            this.currentPage = 1;
            this.renderPage(this.currentPage);
        });
    },

    renderPage(num) {
        if (!this.pdfDoc) return;
        this.pdfDoc.getPage(num).then(page => {
            let canvas = document.getElementById(this.canvasId);
            let ctx = canvas.getContext('2d');
            let viewport = page.getViewport({scale: 1});

            canvas.width = viewport.width;
            canvas.height = viewport.height;

            let renderContext = {
                canvasContext: ctx,
                viewport
            };

            page.render(renderContext);
            document.getElementById("pageInfo").innerText = `Page ${num} / ${this.totalPages}`;
        });
    },

    nextPage() {
        if (this.currentPage < this.totalPages) {
            this.currentPage++;
            this.renderPage(this.currentPage);
        }
    },

    prevPage() {
        if (this.currentPage > 1) {
            this.currentPage--;
            this.renderPage(this.currentPage);
        }
    }
};

window.downloadExcelFile = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName;
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}

window.downloadCsvFile = async (fileName, dotNetStreamRef) => {
    const arrayBuffer = await dotNetStreamRef.arrayBuffer();
    const blob = new Blob([arrayBuffer], { type: 'text/csv;charset=utf-8;' });

    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    link.setAttribute('download', fileName);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

