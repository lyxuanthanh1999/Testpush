import React, { Component } from 'react'
import { SafeAreaView, Text, View } from 'react-native'
import {Provider} from 'react-redux';
import store from './src/redux/Store';
import Main from './src/screens/main'

export default class App extends Component {
  render() {
    return (
      <Provider store={store}>
         <Main/>
      </Provider>
    )
  }
}
