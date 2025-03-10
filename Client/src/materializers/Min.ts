import {processArguments} from "../ProcessArguments";
import {startCursor} from "../StartCursor";

export function min(json: string) : Promise<number> {
    return new Promise(async (resolve, reject) => {
        const args = processArguments(json);

        const request = await startCursor(args.databaseName, args.currentVersion, args.objectStoreName);
        let min : number | null = null;

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                if (min === null) {
                    reject('Source contains no elements.');
                    return;
                }
                resolve(min);
                return;
            }

            const object = cursor.value;
            if (args.matches(object)) {
                const potentialMin = args.selector(object) as number;
                if (min === null) {
                    min = potentialMin;
                }
                if (potentialMin < min) {
                    min = potentialMin;
                }
            }

            cursor.continue();
        });
    });
}