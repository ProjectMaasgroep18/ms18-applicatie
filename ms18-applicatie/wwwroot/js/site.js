// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



// DEMO SCRIPT

const BASE_URL = '/api/v1/';
const LOAD_MSG = document.querySelector('#loading');
const ERROR_MSG = document.querySelector('#error');


function hideElement(el) {
    // Add "hidden" class to element

    el.classList.add('hidden');
}

function showElement(el) {
    // Remove "hidden" class from element
    
    el.classList.remove('hidden');
}

function setLoadMessage(msg) {
    // Set new message to "loading" element

    LOAD_MSG.innerText = msg || "Laden...";
}

function handleError(message) {
    // Show error message

    hideElement(LOAD_MSG);
    showElement(ERROR_MSG);
    ERROR_MSG.innerText = message;
    return Promise.reject(message);
}

async function apiGet(action, _fetchData, _skipJson) {
    // Send a Get request to the API (no request body, with response body)
    // do not use _fetchData and _skipJson directly; it is used by the other API functions

    _fetchData = (_fetchData && typeof _fetchData == 'object') ? _fetchData : {
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json",
        },
        redirect: "follow",
    };

    showElement(LOAD_MSG);
    const json = await fetch(BASE_URL + action, _fetchData).catch(handleError).then(response => {
        if (response.status >= 400 && response.status < 600) {
            // Server error
            return response.text().then(rawdata => {
                try {
                    return handleError(JSON.parse(rawdata).message || 'Er is een onbekende fout opgetreden'); // Foutstatus, maar JSON zonder error message? 
                } catch {
                    console.warn('Failed to parse JSON data', rawdata);
                    return handleError('Er is een onverwachte fout opgetreden'); // Geen geldige JSON (error in plaintext? zie console)
                }
            });
        }
        return _skipJson ? true : response.json().catch(handleError);
    });
    hideElement(LOAD_MSG);
    return json;
}

async function apiPost(action, data) {
    // Send a Post request to the API (with request body and response body)

    const fetchData = {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json",
        },
        redirect: "follow",
        body: JSON.stringify(data),
    };
    return await apiGet(action, fetchData);
}

async function apiPut(action, data) {
    // Send a Put request to the API (with request body, no response body)

    const fetchData = {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
        },
        redirect: "follow",
        body: JSON.stringify(data),
    };
    return await apiGet(action, fetchData, true);
}

async function apiDelete(action) {
    // Send a Delete request to the API (no request body, no response body)

    const fetchData = {
        method: "DELETE",
        redirect: "follow",
    };
    return await apiGet(action, fetchData, true);
}

function showOutput(data, container) {
    // Show output data in a container element

    const transforms = {
        // Functions to "transform" the value (e.g. format a currency value or generate a URL from an ID)

        euro(amount) {
            // Format number to Euro value: 1.95 -> € 1,95
            if (isNaN(+amount))
                return amount;
            return (new Intl.NumberFormat('nl-NL', { style: 'currency', currency: 'EUR' })).format(amount);
        },
        editUrl(id) {
            // Return Edit url of Receipt id
            return '/Declaraties/Edit/' + id;
        },
    };

    showElement(container);
    for (key in data) {
        container.querySelectorAll('[rel="' + key + '"],[rel^="' + key + ':"]').forEach(output => {
            const [key, prop, transform] = output.getAttribute('rel').split(':');
            const value = typeof data[key] == 'object' ? JSON.stringify(data[key]) : data[key];
            const transformedData = (transform && typeof transforms[transform] == 'function') ? transforms[transform](value) : value;
            if (typeof prop == 'undefined' || prop == '') {
                // No property provided
                output.innerText = transformedData;
            } else if (prop.slice(0, 5) == 'data-') {
                // Dataset property ('data-test-test' => output.dataset.testTest)
                output.dataset[prop.slice(5).replace(/-[a-z]/g, substr => substr[1].toUpperCase())] = transformedData;
            } else {
                // Regular property
                output[prop] = transformedData;
            } 
        });
    }
}

async function resizeImage(file, maxSize) {
    // Resize an image and push an <img> to the imgContainer element
    // based on: https://stackoverflow.com/questions/23945494/use-html5-to-resize-an-image-before-upload

    return new Promise(resolve => {
            
        if (!file || typeof file != 'object' || !file.type.match(/^image\//))
            return resolve(null);
        
        let reader = new FileReader();
        let filename = file.name;

        reader.onload = readerEvent => {
            var image = new Image();
            image.onload = () => {
                // Resize the image

                let canvas = document.createElement('canvas'),
                    width = image.width,
                    height = image.height;
                if (width > height) {
                    if (width > maxSize) {
                        height *= maxSize / width;
                        width = maxSize;
                    }
                } else {
                    if (height > maxSize) {
                        width *= maxSize / height;
                        height = maxSize;
                    }
                }
                canvas.width = width;
                canvas.height = height;
                canvas.getContext('2d').drawImage(image, 0, 0, width, height);

                let resizedImage = canvas.toDataURL('image/jpeg');
                return resolve({ filename, data: resizedImage });
            };
            image.src = readerEvent.target.result;
        };
        reader.readAsDataURL(file);
    });
}

function dropContainer(container) {
    // Drop containers (thanks Kevin!)

    const fileInput = container.querySelector('input[type="file"]');

    container.addEventListener("dragover", (e) => {
        // prevent default to allow drop
        e.preventDefault()
    }, false);

    container.addEventListener("dragenter", () => {
        container.classList.add("drag-active")
    });

    container.addEventListener("dragleave", () => {
        container.classList.remove("drag-active")
    });

    container.addEventListener("drop", (e) => {
        e.preventDefault()
        container.classList.remove("drag-active")
        fileInput.files = e.dataTransfer.files;
        fileInput.dispatchEvent(new Event('change'));
    });

    container.addEventListener("click", () => fileInput.click());

    fileInput.addEventListener("change", async () => {
        // Handle file selection

        const files = fileInput.files;
        const imgDiv = container.querySelector('.drop-image-div');
        const title = container.querySelector('.drop-title');
        const max_size = 800; // Maximum 800px (width or height)

        let filesData = [];
        imgDiv.innerHTML = ''; // Clear imgDiv (in case new files were chosen)
    
        for (let i = 0; i < files.length; i++) {
            // Resize the selected images

            let fileData = await resizeImage(files[i], max_size);
            filesData.push(fileData);
            
            // Put the resized image in an img tag
            let img = document.createElement('img');
            img.className = 'drop-image';
            img.dataset.filename = fileData.filename;
            img.src = fileData.data;
            imgDiv.appendChild(img);
        }

        let event = new CustomEvent('photos-resized', { detail: filesData });
        container.dispatchEvent(event);

        title.innerText = filesData.map(data => data.filename).join(', ');
    });
}

// Init drop containers
document.querySelectorAll('.drop-container').forEach(dropContainer);