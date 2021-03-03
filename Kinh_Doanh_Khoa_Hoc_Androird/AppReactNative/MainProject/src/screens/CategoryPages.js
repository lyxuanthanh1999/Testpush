import React, { Component } from 'react'
import { Text, View,FlatList, SafeAreaView } from 'react-native'

import CategoryPageItem from '../compoments/CategoryPageItem'
import Category from '../compoments/Category';

import AppDimensions from '../helpers/AppDimensions';
import { useEffect, useState } from 'react'



export default function CategoryPage ({navigation,route}) {
    useEffect(() => {
    });
        return (
            <Category navigation={navigation} />
        )
    }
