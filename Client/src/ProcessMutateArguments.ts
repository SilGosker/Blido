export function processMutateArguments(json: string): SingleMutateArguments | BatchMutateArguments {
    const obj = JSON.parse(json);

    // If the object has an entity property, it is a SingleMutateArguments object
    if (obj.entity) {
        return {
            database: obj.database,
            version: obj.version,
            entity: JSON.parse(obj.entity),
            objectStore: obj.objectStore,
            primaryKeys: obj.primaryKeys
        };
    }

    return {
        database: obj.database,
        version: obj.version,
        objectStores: obj.objectStores.map((x: {
            entities: {
                entity: string,
                state: string
            }[],
            objectStore: string,
            primaryKeys: string[]
        }) => ({
            objectStore: x.objectStore,
            entities: x.entities.map(y => ({
                entity: JSON.parse(y.entity),
                state: y.state
            })),
            primaryKeys: x.primaryKeys
        }))
    };
}

export interface SingleMutateArguments {
    primaryKeys: string[];
    database: string,
    version: number,
    objectStore: string,
    entity: object
}

export interface BatchMutateArguments {
    database: string,
    version: string,
    objectStores: BatchObjectStoreMutateArguments[]
}

export interface BatchObjectStoreMutateArguments {
    objectStore: string,
    entities: BatchEntityMutateArguments[],
    primaryKeys: string[],
}

export interface BatchEntityMutateArguments {
    entity: unknown,
    state: "Added" | "Modified" | "Deleted"
}