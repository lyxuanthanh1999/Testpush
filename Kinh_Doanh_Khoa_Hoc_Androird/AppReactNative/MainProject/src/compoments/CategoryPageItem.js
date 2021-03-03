import React, {Component} from 'react';
import {Text, View, StyleSheet, TouchableOpacity,Modal,Image, ImageBackground} from 'react-native';
import AppDimensions from '../helpers/AppDimensions';

export default class CategoryPageItem extends Component {
    constructor(props) {
        super(props);
        this.state = {
          modalVisible: false,
        };
      }
      componentDidMount(){
         console.log('du lieu categoryItem : '+JSON.stringify(this.props.item.categoryViewModels))
      }
      renderItemCategoryPage = (category,index,navigation) => {
        return (
            <TouchableOpacity onPress={()=>{
              navigation.navigate('CategoryPageParen',{data:category.categoryViewModels})
            }} style={{flex:1,paddingTop:5,paddingHorizontal:5,paddingVertical:5}}>
                <View style={{backgroundColor:index%2===0?'rgba(125, 222, 169, 0.9)':'rgba(204, 125, 222, 0.9)',borderRadius:25,width:'100%',height:200,alignItems:'center',justifyContent:'center'}}>
                    <Text style={{fontSize:15,fontWeight:'bold'}}>
                        {category.name}
                    </Text>
                </View>
            </TouchableOpacity>
        );
      };
    render() {
        return  this.renderItemCategoryPage(this.props.item,this.props.index,this.props.navigation);
    }
}
