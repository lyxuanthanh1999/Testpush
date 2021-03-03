import React, { Component } from 'react'
import { Text, View,Dimensions,FlatList, Alert } from 'react-native'
import AppDimensions from '../helpers/AppDimensions'
import { connect } from 'react-redux';
import ActionCreator from '../redux/Action/ActionCreator';
import CourseItem from './CourseItem';

class CoursesList extends Component {
    constructor(props)
    {
        super(props);
        this.state = {
            page : 1,
            data:[],
            loading:false,

        }
        
    }
    render() {
        return (
            <View>
                <Text> textInComponent </Text>
            </View>
        )
    }
}
const mapStateToProps = (state)=>{
    return {courses : state.courses}
}

export default connect(mapStateToProps,ActionCreator)(CoursesList);

