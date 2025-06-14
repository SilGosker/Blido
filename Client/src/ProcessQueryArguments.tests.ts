import { processQueryArguments } from "./ProcessQueryArguments";

describe("processQueryArguments(json)", () => {
    it("should return QueryArguments with default matches function and undefined selector when no filters or selector are provided", () => {
        // Arrange
        const json = JSON.stringify({
            database: "testDB",
            objectStore: "testStore",
            version: 1
        });

        // Act
        const result = processQueryArguments(json);

        // Assert
        expect(result.databaseName).toBe("testDB");
        expect(result.objectStoreName).toBe("testStore");
        expect(result.currentVersion).toBe(1);
        expect(result.matches({})).toBe(true);
        expect(result.matches(undefined)).toBe(true);
        expect(result.selector).toBeUndefined();
        expect(result.hasFilters).toBe(false);
        expect(result.id).toBeUndefined();
    });

    it("should parse filters when parsedExpressions are provided", () => {
        // Arrange
        const json = JSON.stringify({
            database: "testDB",
            objectStore: "testStore",
            version: 1,
            parsedExpressions: [
                "(entity) => entity.age > 18",
                "(entity) => entity.name.startsWith('John')"
            ]
        });

        // Act
        const result = processQueryArguments(json);

        // Assert
        expect(result.hasFilters).toBe(true);
        expect(result.matches({ age: 20, name: "John Doe" })).toBe(true);
        expect(result.matches({ age: 16, name: "Jane Doe" })).toBe(false);
    });

    // Test case with selector
    it("should correctly process selector when parsedSelector is provided", () => {
        // Arrange
        const json = JSON.stringify({
            database: "testDB",
            objectStore: "testStore",
            version: 1,
            parsedSelector: "(entity) => entity.priority"
        });

        // Act
        const result = processQueryArguments(json);

        // Assert
        expect(result.selector({ priority: 5 })).toBe(5);
        expect(result.selector({ priority: 10 })).toBe(10);
    });

    // Test case with ID
    it("should correctly process identifiers", () => {
        // Arrange
        const json = JSON.stringify({
            database: "testDB",
            objectStore: "testStore",
            version: 1,
            identifiers: 123
        });

        // Act
        const result = processQueryArguments(json);

        // Assert
        expect(result.id).toBe(123);
    });

    // Test case with complex filters and selector
    it("should correctly handle complex filters and selector together", () => {
        // Arrange
        const json = JSON.stringify({
            database: "testDB",
            objectStore: "testStore",
            version: 1,
            parsedExpressions: [
                "(entity) => entity.age > 18",
                "(entity) => entity.active === true"
            ],
            parsedSelector: "(entity) => entity.score"
        });

        // Act
        const result = processQueryArguments(json);

        // Assert
        expect(result.hasFilters).toBe(true);
        expect(result.matches({ age: 20, active: true, score: 95 })).toBe(true);
        expect(result.matches({ age: 16, active: true, score: 85 })).toBe(false);
        expect(result.matches({ age: 25, active: false, score: 90 })).toBe(false);
        expect(result.selector({ score: 100 })).toBe(100);
    });

    // Error handling test cases
    it("should throw an error for invalid JSON", () => {
        // Arrange
        const invalidJson = "{ invalid json }";

        // Act & Assert
        expect(() => processQueryArguments(invalidJson)).toThrow();
    });
});