const path = require('path');
const rxPaths = require('rxjs/_esm5/path-mapping');

const webpack = require('webpack');

const HtmlWebpackPlugin = require('html-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const FilterWarningsPlugin = require('webpack-filter-warnings-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

const devMode = process.env.NODE_ENV !== 'production';

var fs = require('fs');

const helpers = require('./webpack.helpers');

const ROOT = path.resolve(__dirname, '..');

console.log('@@@@@@@@@ USING DEVELOPMENT @@@@@@@@@@@@@@@');

module.exports = {
  mode: 'development',
  devtool: 'source-map',
  node: {
    dns: 'mock',
    fs: 'empty',
    crypto: true,
    stream: true,
    net: 'empty',
    tls: 'empty'
  },
  performance: {
    hints: false
  },
  entry: {
    polyfills: './src/polyfills.ts',
    app: ['./src/main.ts', 'webpack/hot/dev-server', './src/app/assets/styles/styles.css']
  },

  output: {
    path: path.resolve(ROOT + '/../wwwroot/'),
    filename: 'dist/[name].bundle.js',
    chunkFilename: 'dist/[id].chunk.js',
    publicPath: '/'
  },

  resolve: {
    extensions: ['.ts', '.js', '.json'],
    modules: [ROOT, 'node_modules'],
    alias: rxPaths()
  },

  devServer: {
    historyApiFallback: true,
    contentBase: path.join(ROOT, '/wwwroot/'),
    hot: true,
    https: {
      key: fs.readFileSync(path.resolve(ROOT + '/../Certificates/server.key')),
      cert: fs.readFileSync(path.resolve(ROOT + '/../Certificates/Angular8Core3Sample.crt')),
      ca: fs.readFileSync(path.resolve(ROOT + '/../Certificates/Angular8Core3Sample.pem')),
      passphrase: 'grfla87936'
    },
    port: 8080,
    watchOptions: {
      aggregateTimeout: 300,
      poll: 1000
    }
  },

  module: {
    rules: [
      {
        test: /\.ts$/,
        loaders: [
          'awesome-typescript-loader',
          'angular-router-loader',
          'angular2-template-loader',
          'source-map-loader',
          'tslint-loader'
        ]
      },
      {
        test: /\.(png|jpg|gif|woff|woff2|ttf|svg|eot)$/,
        loaders: 'file-loader?name=assets/[name]-[hash:6].[ext]'
      },
      {
        test: /favicon.ico$/,
        loaders: 'file-loader?name=/[name].[ext]'
      },
      {
        test: /\.less$/,
        loaders: [{
          loader: 'style-loader'
        }, {
          loader: 'css-loader', options: {
              sourceMap: true
          }
        }, {
          loader: 'less-loader', options: {
              sourceMap: true
          }
        }]
      },
      {
          test: /\.css$/,
          include: [path.join(ROOT, 'src', 'app')],
          use: 'raw-loader'
      }, {
          test: /\.css$/,
          exclude: [path.join(ROOT, 'src' , 'app', 'components')],
          use: [MiniCssExtractPlugin.loader, "css-loader"]
      },
      {
          test: /\.html$/,
          loader: 'raw-loader'
      }
    ],
    exprContextCritical: false
  },
  plugins: [
    function () {
      this.plugin('watch-run', function (watching, callback) {
        console.log(
          '\x1b[33m%s\x1b[0m',
          `Begin compile at ${new Date().toTimeString()}`
        );
        callback();
      });
    },

    new webpack.optimize.ModuleConcatenationPlugin(),

    new webpack.ProvidePlugin({
      $: 'jquery',
      jQuery: 'jquery',
      'window.jQuery': 'jquery'
    }),

    new webpack.ProvidePlugin({
      Quill: 'Quill'
    }),

    // new webpack.optimize.CommonsChunkPlugin({ name: ['vendor', 'polyfills'] }),

    new HtmlWebpackPlugin({
      filename: 'index.html',
      inject: 'body',
      favicon: './src/assets/images/favicon.ico',
      template: './src/index.html'
    }),

    new CopyWebpackPlugin(),

    new CleanWebpackPlugin(),

    new FilterWarningsPlugin({
      exclude: /System.import/
    }),

      new MiniCssExtractPlugin({
          // Options similar to the same options in webpackOptions.output
          // both options are optional
          filename: devMode ? '[name].css' : '[name].[hash].css',
          chunkFilename: devMode ? '[id].css' : '[id].[hash].css',
      }),

    // OccurrenceOrderPlugin is needed for webpack 1.x only
    new webpack.optimize.OccurrenceOrderPlugin(),
    new webpack.HotModuleReplacementPlugin(),
    // Use NoErrorsPlugin for webpack 1.x
    new webpack.NoEmitOnErrorsPlugin()
  ]
};

