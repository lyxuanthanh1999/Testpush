import React, { Component } from 'react'
import { Text, View,FlatList,Dimensions } from 'react-native'
import AppDimensions from '../helpers/AppDimensions';
import CategoryPageItem from './CategoryPageItem';

import { connect } from 'react-redux';
import ActionCreator from '../redux/Action/ActionCreator';

class Category extends Component {
    constructor(props) {
        super(props);
    }
    componentDidMount(){
        // this.props.fetchCourses();
        this.props.fetchCategories();
        // console.log(this.props.fetchCategories());
    }
    render() {
        return (
            <View style={{backgroundColor:'black',height:Dimensions.get('screen').height}}>
            <View style={{alignItems:'center',justifyContent:'center'}}>
                    <View style={{width:AppDimensions.getWith()}}>
                        <FlatList
                        numColumns={2}
                        style={{width:'100%',paddingHorizontal:5}}
                        data={this.props.categories}
                        keyExtractor={(item, index) => item.id.toString()}
                        renderItem={({item,index}) => (
                            <CategoryPageItem
                            navigation={this.props.navigation}
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
}

const mapStateToProps = (state)=>{
    return {categories : state.categories}
}
export default connect(mapStateToProps,ActionCreator)(Category);