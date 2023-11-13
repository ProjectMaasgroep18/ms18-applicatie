// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



// DEMO SCRIPT

const BASE_URL = '/api/v1/';
const LOAD_MSG = document.querySelector('#loading');
const ERROR_MSG = document.querySelector('#error');


function hideElement(el) {
    // Add "hidden" class to element

    if (!el.className.match(/(\s|^)hidden(\s|$)/))
        el.className = (el.className.trim() + ' hidden').trim();
}

function showElement(el) {
    // Remove "hidden" class from element
    
    el.className = el.className.replace(/(\s+|^)hidden(\s+|$)/, ' ').trim();
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
                    return handleError(JSON.parse(rawdata).message);
                } catch {
                    console.warn('Failed to parse JSON data', rawdata);
                    return handleError('Er is een onverwachte fout opgetreden');
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
                // Dataset property
                output.dataset[prop.slice(5)] = transformedData;
            } else {
                // Regular property
                output[prop] = transformedData;
            } 
        });
    }
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

    fileInput.addEventListener("change", () => {
        // Handle file selection
        // based on: https://stackoverflow.com/questions/23945494/use-html5-to-resize-an-image-before-upload

        const file = fileInput.files[0];
        const img = container.querySelector('.drop-image');
        const title = container.querySelector('.drop-title');
        const reader = new FileReader();
        const max_size = 800; // Maximum 800px (width or height)

        if (file.type.match(/^image\//)) {
            // Load the image

            reader.onload = readerEvent => {
                var image = new Image();
                image.onload = () => {
                    // Resize the image

                    let canvas = document.createElement('canvas'),
                        width = image.width,
                        height = image.height;
                    if (width > height) {
                        if (width > max_size) {
                            height *= max_size / width;
                            width = max_size;
                        }
                    } else {
                        if (height > max_size) {
                            width *= max_size / height;
                            height = max_size;
                        }
                    }
                    canvas.width = width;
                    canvas.height = height;
                    canvas.getContext('2d').drawImage(image, 0, 0, width, height);
                    resizedImage = canvas.toDataURL('image/jpeg');

                    // Now let's something with the resized image
                    img.src = resizedImage;
                    showElement(img);
                    title.innerText = file.name;

                };
                image.src = readerEvent.target.result;
            };
            reader.readAsDataURL(file);
        } else {
            // Not an image, discard it

            fileInput.value = null;
        }
    });
}

// Init drop containers
document.querySelectorAll('.drop-container').forEach(dropContainer);