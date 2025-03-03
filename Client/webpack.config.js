const path = require('path');
const TerserPlugin = require('terser-webpack-plugin');

module.exports = {
    mode: 'development', // Use 'development' for easier debugging
    entry: './src/Load.ts', // Entry point of your TypeScript file
    output: {
        filename: 'load.js', // Output file name
        path: path.resolve(__dirname, 'dist'), // Output directory
        library: 'BlazorIndexedOrm', // Exported library name (optional)
        libraryTarget: 'var', // Make it accessible globally in browsers
    },
    resolve: {
        extensions: ['.ts', '.js'], // Resolve these extensions
    },
    module: {
        rules: [
            {
                test: /\.ts$/, // Match TypeScript files
                use: 'ts-loader',
                exclude: /node_modules/,
            },
        ],
    },
    optimization: {
        minimize: true, // Enable minification
        minimizer: [new TerserPlugin()],
    },
};
