const prod = process.env.NODE_ENV === 'production';
const HtmlWebpackPlugin = require('html-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const {resolve} = require("path");

module.exports = {
    mode: prod ? 'production' : 'development',
    target: 'web',
    entry: './src/index.tsx',
    output: {
        path: resolve(__dirname, "dist"),
        publicPath: '/',
        filename: "bundle.js"
    },
    module: {
        rules: [
            {
                test: /\.(ts|tsx)$/,
                exclude: /node_modules/,
                resolve: {
                    extensions: ['.ts', '.tsx', '.js', '.jsx', '.json'],
                },
                use: ['babel-loader', 'ts-loader'],
            },
            {
                test: /\.css$/,
                use: [MiniCssExtractPlugin.loader, 'css-loader'],
            },
        ]
    },
    devtool: 'source-map',
    plugins: [
        new HtmlWebpackPlugin({
            template: 'index.html',
        }),
        new MiniCssExtractPlugin(),
    ],
//     externals: {
// //        'ConfigurationValues': JSON.stringify(process.env.NODE_ENV === 'production' ? resolve(__dirname, 'configuration/config.prod.js') : resolve(__dirname, 'configuration/config.dev.js'))
//     },
};
