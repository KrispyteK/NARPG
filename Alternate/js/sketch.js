let capture
let img
let oldGrayScale = []
let theshold = 25

function setup() { 
    createCanvas(400, 400);
    capture = createCapture(VIDEO);
    capture.size(400, 300);
    img = createImage(400,300)
}

function draw() { 
    background(0)

    img.loadPixels()
    capture.loadPixels()

    for (let x = 0; x < capture.width; x++) {
        for (let y = 0; y < capture.height; y++) {
            let index = ((y*capture.width) + x) * 4
            let r = capture.pixels[index]
            let g = capture.pixels[index + 1]
            let b = capture.pixels[index + 2]

            let grayScale = (r + g + b) / 3
            let difference = Math.abs(grayScale - oldGrayScale[index])
            let value = difference > theshold ? 255 : 0

            img.pixels[index] = value
            img.pixels[index+1] = value
            img.pixels[index+2] = value
            img.pixels[index+3] = 255;

            oldGrayScale[index] = grayScale
        }
    }

    img.updatePixels()

    image(img,0,0,400,300)
}