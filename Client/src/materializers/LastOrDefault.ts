import {processArguments} from "../ProcessArguments";
import {startCursor} from "../StartCursor";

export function lastOrDefault(json: string) : Promise<unknown | null> {
    return new Promise(async (resolve, reject) => {
        const args = processArguments(json);
        const request = await startCursor(args.databaseName, args.currentVersion, args.objectStoreName);
        let last: unknown | null = null;

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                resolve(last);
                return;
            }

            const object = cursor.value;
            if (args.matches(object)) {
                last = object;
            }

            cursor.continue();
        });
    });
}