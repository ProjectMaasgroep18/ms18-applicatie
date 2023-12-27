// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



// Script for web front-end

const BASE_URL = '/api/v1/';
const LOAD_MSG = document.querySelector('#loading');
const ERROR_MSG = document.querySelector('#error');
const LOGIN_FORM = document.querySelector('#login');
let AFTER_LOGIN = null;


function hideElement(el) {
    // Add "hidden" class to element
    
    if (el.toString() == '[object NodeList]')
        el.forEach(el => el.classList.add('hidden'));
    else if ('classList' in el)
        el.classList.add('hidden');
}

function showElement(el) {
    // Remove "hidden" class from element
    
    if (el.toString() == '[object NodeList]')
        el.forEach(el => el.classList.remove('hidden'));
    else if ('classList' in el)
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

async function apiGet(action, _fetchData, _skipJson, _onValidLogin) {
    // Send a Get request to the API (no request body, with response body)
    // do not use _fetchData,  _skipJson, and _onValidLogin directly; it is used by the other API functions

    _fetchData = (_fetchData && typeof _fetchData == 'object') ? _fetchData : {
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json",
        },
        redirect: "follow",
    };

    let token = localStorage.getItem("AUTH_TOKEN");
    if (token) {
        if (!_fetchData.headers)
            _fetchData.headers = {};
        _fetchData.headers.Authorization = "Bearer " + token;
    }

    showElement(LOAD_MSG);
    const json = await fetch(BASE_URL + action, _fetchData).catch(handleError).then(response => {
        if (response.status >= 400 && response.status < 600) {
            // Server error
            return response.text().then(rawdata => {
                try {
                    if (response.status == 401) {
                        showElement(LOGIN_FORM);
                        LOGIN_FORM?.querySelector('.login-email')?.focus();
                        LOGIN_FORM?.querySelector('.login-email')?.select();
                        if (typeof _onValidLogin == 'function')
                            AFTER_LOGIN = _onValidLogin;
                    }
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

async function requireLogin(onValidLogin) {
    // Make sure user is logged in (and show login screen if they aren't)

    let afterLogin = member => {
        showElement(document.querySelectorAll('.login-only'));
        let userCircle = document.querySelector('#user-circle');
        if (userCircle) {
            userCircle.style.width = '24px';
            userCircle.style.height = '24px';
            userCircle.style.borderRadius = '24px';
            userCircle.style.fontSize = '15px';
            userCircle.style.fontWeight = '700';
            userCircle.style.textAlign = 'center';
            userCircle.style.color = '#FFF';
            userCircle.style.textShadow = '#000 0 0 3px';
            userCircle.style.backgroundColor = member.color;
            userCircle.innerText = member.name.length > 0 ? member.name[0] : "";
        }
        let tokenEl = document.querySelector('#login-token');
        if (tokenEl)
            tokenEl.innerText = localStorage.getItem("AUTH_TOKEN");
        if (typeof onValidLogin == 'function')
            onValidLogin(member);
    };
    let result = await apiGet('User/Current', null, null, afterLogin);
    
    // If we got here, it worked
    afterLogin(result);
    return result;
}

async function logout() {
    // Logout and try to remove token from the server

    hideElement(document.querySelectorAll('.login-only'));
    apiGet('User/Logout', null, true).then(_ => window.location.href = '/');
    localStorage.removeItem("AUTH_TOKEN");
}

function showOutput(data, container) {
    // Show output data in a container element

    const transforms = {
        // Functions to "transform" the value (e.g. format a currency value or generate a URL from an ID)

        euro(amount) {
            // Format number to Euro value: 1.95 -> € 1,95
            if (isNaN(+amount) || amount === null)
                return amount;
            return (new Intl.NumberFormat('nl-NL', { style: 'currency', currency: 'EUR' })).format(amount);
        },
        editUrl(id) {
            // Return Edit url of Receipt id
            return '/Declaraties/Aanpassen/' + id;
        },
        viewUrl(id) {
            // Return View url of Receipt id
            return '/Declaraties/Bekijken/' + id;
        },
        productUrl(id) {
            // Return Product url of Product id
            return '/Producten/Aanpassen/' + id;
        },
        userUrl(id) {
            // Return User url of User id
            return '/Ledenbeheer/Aanpassen/' + id;
        },
        zero(val) {
            // Default to zero instead of "", null, undefined, etc.
            return !val ? 0 : val;
        },
        dateTime(val) {
            // Readable date/time (in browser timezone)
            let date = new Date(val);
            if (date === "Invalid Date" || isNaN(date))
                return val;

            return (date.getDate() < 10 ? '0' : '') + date.getDate()
                + '-' + (date.getMonth() < 9 ? '0' : '') + (date.getMonth() + 1)
                + '-' + date.getFullYear()
                + ' ' + (date.getHours() < 10 ? '0' : '') + date.getHours()
                + ':' + (date.getMinutes() < 10 ? '0' : '') + date.getMinutes();
        },
        commaSeparated(values) {
            // Comma-separated array values
            if (typeof values == 'object' && values && 'join' in values)
                return values.join(', ');
            return values;
        }
    };

    showElement(container);
    const outputElements = container.querySelectorAll('[rel]');

    // console.log('Rendering data in container', container, data);

    outputElements.forEach(output => {
        const rels = output.getAttribute('rel') ? output.getAttribute('rel').split(',') : [];
        rels.forEach(rel => {
            const [key, prop, transform] = rel.split(':');
            let splitKey = key.split('.'); // Support data in structures (rel="key.subkey" with { "key": { "subkey": "value" }} will show "value")
            let keyData = data[splitKey.shift()] ?? null;
            while (splitKey.length > 0 && (keyData ?? null) !== null)
                keyData = keyData[splitKey.shift()] ?? null;
            let transformedData = ((transform && typeof transforms[transform] == 'function') ? transforms[transform](keyData) : keyData) ?? null;
            transformedData = typeof transformedData == 'object' && transformedData !== null ? JSON.stringify(transformedData) : transformedData;
            if ((transformedData ?? '') === '' && (prop ?? '') === '' && output.tagName != 'TEXTAREA')
                transformedData = '\u2014'; // — streepje
            // console.log(key, prop, transform, '=>', transformedData);
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
    });
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

async function apiGetInfinite(action, container, onLoadItems, perPage, page) {
    // Call API get method that supports infinite scroll (i.e., offset/limit parameters)

    const toElement = value => typeof value == 'string' ? document.querySelector(value) : typeof value == 'object' ? value : null;
    container = toElement(container);

    const limit = perPage || 100;
    const offset = ((page || 1) - 1) * limit;
    const params = '?offset=' + encodeURIComponent(offset) + '&limit=' + encodeURIComponent(limit);
    
    const results = await apiGet(action + params);
    if (!container || !('querySelector' in container)) {
        if (typeof onLoadItems == 'function')
            onLoadItems(null, results);
        return results;
    }

    showElement(container);
    
    // Show or hide "no results" message
    const noResults = (!results || results.length == 0) && offset == 0;
    noResults ? showElement(container.querySelectorAll('.result-empty')) : hideElement(container.querySelectorAll('.result-empty'));
    
    const baseItem = container.querySelector('.result-item');
    const newItems = [];
    if (!baseItem) {
        if (typeof onLoadItems == 'function')
            onLoadItems(null, results);
        return results;
    }

    for (i in results) {
        // Generate and populate output elements

        const resultItem = baseItem.cloneNode(true);
        if (!resultItem)
            continue;
        resultItem.classList.add('result-item-loaded');
        resultItem.dataset.id = results[i].id;
        showOutput(results[i], resultItem);
        let allItems = container.querySelectorAll('.result-item-loaded');
        let lastItem = allItems.length ? allItems[allItems.length - 1] : baseItem;
        lastItem.insertAdjacentElement('afterend', resultItem);
        newItems.push(resultItem);
    }

    hideElement(baseItem);

    if (results.length < limit) {
        // No more results
        hideElement(container.querySelectorAll('.result-has-more'));
    } else {
        // There are more results
        const moreLink = container.querySelector('.result-show-more');
        if (moreLink) {
            const newMoreLink = moreLink.cloneNode(true); // Removes old data
            newMoreLink.addEventListener('click', event => {
                // Load next page of results
                event.preventDefault();
                apiGetInfinite(action, container, onLoadItems, perPage, (page || 1) + 1);
            });
            moreLink.insertAdjacentElement('afterend', newMoreLink);
            moreLink.remove();
        }
        showElement(container.querySelectorAll('.result-has-more'));
    }

    if (typeof onLoadItems == 'function')
        onLoadItems(newItems, results);

    return results;
}

LOGIN_FORM?.querySelector('.login-email')?.addEventListener('keyup', event => {
    if (event.which != 13 || !event.target.value)
        return;
    LOGIN_FORM?.querySelector('.login-password')?.focus();
});

LOGIN_FORM?.querySelector('.login-password')?.addEventListener('keyup', event => {
    if (event.which != 13)
        return;
    LOGIN_FORM?.querySelector('.login-button')?.dispatchEvent(new Event('click'));
});

LOGIN_FORM?.querySelector('.login-button')?.addEventListener('click', () => {
    // Log in, retry whatever we were doing before

    let email = LOGIN_FORM.querySelector('.login-email')?.value;
    let password = LOGIN_FORM.querySelector('.login-password')?.value;
    let guestCheckbox = LOGIN_FORM.querySelector('.login-guest');
    LOGIN_FORM.querySelector('.login-password').value = null;

    console.log(email, password, guestCheckbox);
    
    if (!email)
        return;

    if (!password && guestCheckbox)
        guestCheckbox.checked = true;
    if (guestCheckbox?.checked)
        password = '';
    
    hideElement(LOGIN_FORM);

    apiPost('User/Login', { email, password }).then(result => {
        hideElement(LOGIN_FORM);
        hideElement(ERROR_MSG);
        if (result.token)
            localStorage.setItem("AUTH_TOKEN", result.token);
        if (typeof AFTER_LOGIN == 'function' && result.member)
            AFTER_LOGIN(result.member);
        AFTER_LOGIN = null;
    });
});