const path = require("path");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = (env = {}, argv = {}) => {

    const isProd = argv.mode === 'production';

    const config = {

        mode: argv.mode || 'development',
       
        entry: {
            base: "./src/js/base.js"
        },

        output: {
            path: path.resolve(__dirname, "dist"),
            filename: 'js/[name].js'
        },
        devtool: isProd ? 'source-map' : false,

        plugins: [
            new CleanWebpackPlugin(),

            new MiniCssExtractPlugin({
                filename: 'css/[name].css'                
            })
        ],

        module: {
            rules: [
                {
                    test: /\.css$/,
                    use: [
                        {
                            loader: MiniCssExtractPlugin.loader,
                            options: {
                                
                            }
                        },
                        'css-loader'

                    ]
                }/*,
                {
                    test: /\.(png|woff|woff2|eot|ttf|svg)$/,
                    loader: 'file-loader',
                    include: path.resolve(__dirname, 'src')
                }*/
            ]
        }
    }

    return config;
};