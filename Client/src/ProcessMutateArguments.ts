
export function ProcessMutateArguments(json: string) : SingleMutateArguments | BatchMutateArguments {
    const obj = JSON.parse(json);
    if (obj.entity) {
        return {
            database: obj.database,
            version: obj.version,
            entity: JSON.parse(obj.entity),
            objectStore: obj.objectStore,
            primaryKeyFields: obj.primaryKeys
        };
    }

    return {
        database: obj.database,
        version: obj.version,
        objectStores: obj.objectStores.map((x: {entities: {entity: string, state: string}[], objectStore: string}) => ({
            objectStore: x.objectStore,
            entities: x.entities.map(y => ({
                entity: JSON.parse(y.entity),
                state: y.state
            }))
        }))
    };
}

export interface SingleMutateArguments {
    primaryKeyFields: string[] | string;
    database: string,
    version: number,
    objectStore: string,
    entity: unknown
}

export interface BatchMutateArguments {
    database: string,
    version: string,
    objectStores: BatchObjectStoreMutateArguments[]
}

export interface BatchObjectStoreMutateArguments {
    objectStore: string,
    entities: BatchEntityMutateArguments[]
}

export interface BatchEntityMutateArguments {
    entity: unknown,
    state: "Added" | "Modified" | "Deleted"
}