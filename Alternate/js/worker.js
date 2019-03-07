this.onmessage = function (e) {
    let { stepSize,
        startX,
        startY,
        width,
        color,
        pixels } = e.data

    for (let x = startX; x < (startX + stepSize); x++) {
        for (let y = startY; y < (startY + stepSize); y++) {
            let index = ((y * width) + x) * 4

            pixels[index] = color[0]
            pixels[index + 1] = color[1] 
            pixels[index + 2] = color[2]            
            pixels[index + 3] = 255
        }
    }

    this.postMessage({ id: e.data.id, pixels: pixels });
};