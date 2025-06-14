import {BlidoContext} from "./Load";

describe("Load", () => {
    it("Should load the module", async () => {
        // Act
        const blido = new BlidoContext();

        // Assert
        expect(blido.all).toBeDefined();
        expect(blido.any).toBeDefined();
        expect(blido.average).toBeDefined();
        expect(blido.count).toBeDefined();
        expect(blido.first).toBeDefined();
        expect(blido.firstOrDefault).toBeDefined();
        expect(blido.last).toBeDefined();
        expect(blido.lastOrDefault).toBeDefined();
        expect(blido.max).toBeDefined();
        expect(blido.min).toBeDefined();
        expect(blido.single).toBeDefined();
        expect(blido.singleOrDefault).toBeDefined();
        expect(blido.sum).toBeDefined();
        expect(blido.toArray).toBeDefined();
        expect(blido.find).toBeDefined();
        expect(blido.insert).toBeDefined();
    });
});