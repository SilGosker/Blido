import {processMutateArguments, SingleMutateArguments} from "../ProcessMutateArguments";
import {startTransaction} from "../StartTransaction";

export async function insert(json: string) {
    return new Promise<unknown>(async (resolve, reject) => {
        const args = processMutateArguments(json) as SingleMutateArguments;
        const objectStore = await startTransaction(args.database, args.version, args.objectStore, false);

        const request = objectStore.add(args.entity);

        request.onsuccess = () => {
            const entity = {...args.entity} as { [key: string]: unknown };

            if (request.result instanceof Array) {
                console.warn("multiple array keys aren't fully supported yet and the order of them can't be guaranteed");
                for (let i = 0; i < args.primaryKeys.length; i++) {
                    const pk = args.primaryKeys[i];
                    entity[pk] = request.result[i];
                }
            } else {
                entity[args.primaryKeys[0]] = request.result;
            }
            resolve(entity);
            return;
        };

        request.onerror = () => {
            reject(request.error)
        };
    });
}