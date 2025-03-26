import {processQueryArguments} from "../ProcessQueryArguments";
import {startCursor} from "../StartCursor";

export function max(json: string) : Promise<number> {
    return new Promise(async (resolve, reject) => {
        const args = processQueryArguments(json);

        const request = await startCursor(args.databaseName, args.currentVersion, args.objectStoreName);
        let max : number | null = null;

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                if (max === null) {
                    reject('Source contains no elements.');
                    return;
                }
                resolve(max);
                return;
            }

            const object = cursor.value;
            if (args.matches(object)) {
                const potentialMax = args.selector(object) as number;
                if (max === null) {
                    max = potentialMax;
                }
                if (potentialMax > max) {
                    max = potentialMax;
                }
            }

            cursor.continue();
        });
    });
}