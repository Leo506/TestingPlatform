function set(key, value) {
    localStorage.setItem(key, value);
}

function get(key) {
    return localStorage.getItem(key);
}

function remove(key) {
    return  localStorage.removeItem(key);
}

function ConfirmAction(message) {
    return confirm(message);
}