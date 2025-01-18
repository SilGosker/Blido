export function connectToDatabase(name, currentVersion) {
    return new Promise(function (resolve, reject) {
        var request = window.indexedDB.open(name, currentVersion);
        request.addEventListener('success', resolve);
        request.addEventListener('error', reject);
    });
}
