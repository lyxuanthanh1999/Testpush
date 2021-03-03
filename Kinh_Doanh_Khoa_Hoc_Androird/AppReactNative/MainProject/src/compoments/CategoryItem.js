import React, {Component} from 'react';
import {Text, View, StyleSheet, TouchableOpacity,Modal,Image, ImageBackground} from 'react-native';
import AppDimensions from '../helpers/AppDimensions';

export default class CategoryItem extends Component {
    constructor(props) {
        super(props);
        this.state = {
          modalVisible: false,
        };
      }
      renderItemCategory = (category,index) => {
        return (
            <View style={{flexDirection:'column',flex:1, borderRadius:25,width:AppDimensions.getWith(),marginRight:10,borderWidth:2,borderColor:'white',backgroundColor:index%2===0?'rgba(84, 84, 232, 0.9)':'rgba(84, 232, 210, 0.9)'}}>
                                <View style={{flexDirection:'column'}}>
                                    <Image  source={category.Image} style={{
                                    borderRadius:25,
                                    height:AppDimensions.getHeight()/2,width:AppDimensions.getWith(),
                                        resizeMode: "cover",
                                        justifyContent: "center"}}/>
                                </View>
                                <View style={{flex:1,flexDirection:'row',width:AppDimensions.getWith()}}>
                                        <View style={{width:'50%',justifyContent:'center',alignItems:'flex-start'}}>
                                            <Text style={{fontSize:20,fontWeight:'bold',textTransform:'uppercase'}}>{category.Name}</Text>
                                        </View>
                                        <View style={{width:'50%',justifyContent:'center',alignItems:'center',marginLeft:20}}>
                                            <TouchableOpacity style={{height:50,width:150,borderColor:'black',borderRadius:25,borderWidth:2,justifyContent:'center',alignItems:'center'}}> 
                                                <Text style={{fontSize:20,}}>Xem Chi Tiết</Text>
                                            </TouchableOpacity>
                                        </View>
                                </View>
            </View> 
        );
      };
    render() {
        return this.renderItemCategory(this.props.item,this.props.index);
    }
}
