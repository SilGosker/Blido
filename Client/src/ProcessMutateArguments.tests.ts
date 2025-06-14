import {processMutateArguments} from "./ProcessMutateArguments";

describe("processMutateArguments(json)", () => {

    it("should return a SingleMutateArguments object when the input JSON has an entity property", () => {
        // Arrange
        const json = JSON.stringify({
            database: "db",
            version: 1,
            entity: JSON.stringify({ id: 1 }),
            objectStore: "store",
            primaryKeys: ["id"]
        });

        // Act
        const result = processMutateArguments(json);

        // Assert
        expect(result).toEqual({
            database: "db",
            version: 1,
            entity: { id: 1 },
            objectStore: "store",
            primaryKeys: ["id"]
        });
    });

    it("should return a BatchMutateArguments object when the input JSON has an objectStores property", () => {
        // Arrange
        const json = JSON.stringify({
            database: "db",
            version: 1,
            objectStores: [
                {
                    objectStore: "store",
                    entities: [
                        {
                            entity: JSON.stringify({ id: 1 }),
                            state: "Added"
                        }
                    ],
                    primaryKeys: ["id"]
                }
            ]
        });

        // Act
        const result = processMutateArguments(json);

        // Assert
        expect(result).toEqual({
            database: "db",
            version: 1,
            objectStores: [
                {
                    objectStore: "store",
                    entities: [
                        {
                            entity: { id: 1 },
                            state: "Added"
                        }
                    ],
                    primaryKeys: ["id"]
                }
            ]});
    });
});