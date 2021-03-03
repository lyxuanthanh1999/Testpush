import React, { Component } from 'react'
import { Text, View,FlatList, SafeAreaView, Dimensions } from 'react-native'
import CategoryPageItem from '../compoments/CategoryPageItem'
import CategoryPageItem1 from '../compoments/CategoryPageItem1'
import AppDimensions from '../helpers/AppDimensions';
import { useEffect, useState } from 'react'

export default function CategoryPageParen ({navigation,route}) {
    const [DATA, setDATA] = useState([])
    useEffect(() => {
        console.log('CategoryPageParen===>'+JSON.stringify(route.params.data))
        setDATA(route.params.data)
    })
        return (
            <View style={{backgroundColor:'black',height:Dimensions.get('window').height}}>
            <View style={{alignItems:'center',justifyContent:'center'}}>
                    <View style={{width:AppDimensions.getWith()}}>
                        <FlatList
                        numColumns={2}
                        style={{width:'100%',paddingHorizontal:5}}
                            // numColumns={2}
                        // horizontal={true}
                        data={DATA}
                        keyExtractor={(item, index) => item.id.toString()}
                        renderItem={({item,index}) => (
                            <CategoryPageItem1
                            item={item}
                            index={index}
                            />
                        )}
                        />  
                    </View>
            </View>
        </View>
        )
    }

