// let capture
// let img
// let oldGrayScale = []
// let theshold = 25

// function setup() { 
//     createCanvas(400, 400);
//     capture = createCapture(VIDEO);
//     capture.size(400, 300);
//     img = createImage(400,300)
// }

// function draw() { 
//     background(0)

//     img.loadPixels()
//     capture.loadPixels()

//     for (let x = 0; x < capture.width; x++) {
//         for (let y = 0; y < capture.height; y++) {
//             let index = ((y*capture.width) + x) * 4
//             let r = capture.pixels[index]
//             let g = capture.pixels[index + 1]
//             let b = capture.pixels[index + 2]

//             let grayScale = (r + g + b) / 3
//             let difference = Math.abs(grayScale - oldGrayScale[index])
//             let value = difference > theshold ? 255 : 0

//             img.pixels[index] = value
//             img.pixels[index+1] = value
//             img.pixels[index+2] = value
//             img.pixels[index+3] = 255;

//             oldGrayScale[index] = grayScale
//         }
//     }

//     img.updatePixels()

//     image(img,0,0,400,300)
// }

let n, worker, running
let capture, img
let oldGrayScale = []
let theshold = 25
let setColor = [255,0,0]
let stepSize = 400
let processedResult

function setup() { 
    createCanvas(400, 400);

    img = createImage(400,300)

    startWorkers()
}

function draw() { 
    //modifyImage()

    image(img,0,0,400,300)
}

function startWorkers () {
    img.loadPixels()

    running = 0
    processedResults = []

    for (let x = 0; x < img.width; x += stepSize) {
        for (let y = 0; y < img.height; y += stepSize) {
            workers = new Worker("js/worker.js");
            workers.onmessage = workerDone;
            workers.postMessage({
                id: running, 
                width: img.width,
                startX: x,
                startY: y,
                stepSize: stepSize,
                color: setColor,
                pixels: img.pixels
            });
            ++running;
        }
    }
}

function workerDone(e) {
    --running;

    processedResults[e.data.id] = e.data.pixels
    e.target.terminate()

    if (running === 0) {
        img.loadPixels()

        let index = 0

        for (let i = 0; i < processedResults.length; i++) {
            let newPixels = processedResults[i]

            for (let j = 0; j < newPixels.length; j++) {
                index++

                img.pixels[index] = newPixels[index]
            }
        }

        img.updatePixels()

        console.log("Workers done!")

        startWorkers()
    }
}

function modifyImage () {
    img.loadPixels()
    let pixels = img.pixels

    for (let x = 0; x < img.width; x++) {
        for (let y = 0; y < img.height; y++) {
            let index = ((y * width) + x) * 4

            pixels[index] = setColor[0]
            pixels[index + 1] = setColor[1] 
            pixels[index + 2] = setColor[2]            
            pixels[index + 3] = 255
        }
    }

    img.updatePixels()
}