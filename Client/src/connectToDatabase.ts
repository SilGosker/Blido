export function connectToDatabase(name: string, currentVersion : number) : Promise<Event> {
    return new Promise((resolve, reject) => {
        const request = window.indexedDB.open(name, currentVersion);
        request.addEventListener('success', resolve);
        request.addEventListener('error', reject);
    });
}