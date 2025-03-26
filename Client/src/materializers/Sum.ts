import {processQueryArguments} from "../ProcessQueryArguments";
import {startCursor} from "../StartCursor";

export function sum(json: string) : Promise<number> {
    return new Promise(async (resolve, reject) => {
        const args = processQueryArguments(json);

        const request = await startCursor(args.databaseName, args.currentVersion, args.objectStoreName);
        let sum = 0;

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                resolve(sum);
                return;
            }

            const object = cursor.value;
            if (args.matches(object)) {
                sum += args.selector(object) as number;
            }

            cursor.continue();
        });
    });
}