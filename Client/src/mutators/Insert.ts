import {ProcessMutateArguments, SingleMutateArguments} from "../ProcessMutateArguments";
import {startTransaction} from "../StartCursor";

export async function insert(json: string) {
    return new Promise<unknown>(async (resolve, reject) => {
        const args = ProcessMutateArguments(json) as SingleMutateArguments;
        const objectStore = await startTransaction(args.database, args.version, args.objectStore, false);

        const request = objectStore.add(args.entity);

        request.onsuccess = () => {
            const entity = args.entity as { [key: string]: unknown };

            if (request.result instanceof Array && args.primaryKeyFields instanceof Array) {
                console.warn("multiple array keys aren't fully supported " +
                                    "yet and the order of them can't be guaranteed");
                for(let i = 0; i < args.primaryKeyFields.length; i++) {
                    const pk = args.primaryKeyFields[i];
                    entity[pk] = request.result[i];
                }
            } else {
                entity[args.primaryKeyFields as string] = request.result;
            }
            resolve(entity);
        };

        request.onerror = reject;
    });
}