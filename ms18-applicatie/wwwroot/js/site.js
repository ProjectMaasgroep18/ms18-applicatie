// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



// DEMO SCRIPT

const BASE_URL = '/api/v1/';
const LOAD_MSG = document.querySelector('#loading');
const ERROR_MSG = document.querySelector('#error');


function handleError(message) {
    // Show error message

    LOAD_MSG.className = 'hidden';
    ERROR_MSG.className = '';
    ERROR_MSG.innerText = message;
    return Promise.reject(message);
}

async function apiGet(action) {
    // Send a Get request to the API

    LOAD_MSG.className = '';
    const json = await fetch(BASE_URL + action).catch(handleError).then(response => {
        if (response.status >= 400 && response.status < 600) {
            // Server error
            return response.text().then(rawdata => {
                try {
                    return handleError(JSON.parse(rawdata).message)
                } catch {
                    console.warn('Failed to parse JSON data', rawdata);
                    return handleError('Er is een onverwachte fout opgetreden');
                }
            });
        }
        return response.json().catch(handleError);
    });
    LOAD_MSG.className = 'hidden';
    return json;
}

function showOutput(data, container) {
    // Show output data in a container element

    container.className = ''; // Unhide
    for (key in data)
        container.querySelectorAll('[rel="' + key + '"]').forEach(output => output.innerText = typeof data[key] == 'object' ? JSON.stringify(data[key]) : data[key]);
}
