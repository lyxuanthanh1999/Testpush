import React, { Component } from 'react'
import { Text, View } from 'react-native'
import Icon from 'react-native-vector-icons/AntDesign'

import HomePage from './HomePage'
import CategoryPage from './CategoryPages'
import Register from './Register'
import CategoryPageParen from './CategoryPageParen'
import ListCoursesPage from './ListCoursesPage'
import DetailCourses from './DetailCourses'
import SuccessPayPage from './SuccessPayPage'
import Search from './Search'
import Login from '../screens/Login'
import account from './Account'
import InforAccount from './InforAccount'
import LessonList from '../compoments/LessonList'
import Courses from '../compoments/Courses'
import CoursesListPage from './CoursesListPage'

import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';

import {connect} from 'react-redux';



const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();
function HomeScreen(){
    return(
        <Stack.Navigator initialRouteName="Home">
                  <Stack.Screen options={{headerShown:false,}} name="Home" component={HomePage} />
                  <Stack.Screen options={{headerShown:false,}} name="Login" component={Login} />
                  <Stack.Screen options={{headerShown:false,}} name="Register" component={Register} />
                  <Stack.Screen options={{headerShown:false,}} name="CategoryPageParen" component={CategoryPageParen} />
                  <Stack.Screen options={{headerShown:false,}} name="ListCoursesPage" component={ListCoursesPage} />
                  <Stack.Screen options={{headerShown:false,}} name="CoursesListPage" component={CoursesListPage} />
                  <Stack.Screen options={{headerShown:false,}} name="Courses" component={Courses} />
                  <Stack.Screen options={{headerShown:false,}} name="DetailCourses" component={DetailCourses} />
                  
                  <Stack.Screen options={{headerShown:false,}} name="Catgories" component={CategoryPage} />
                  <Stack.Screen options={{headerShown:false,}} name="Account" component={account} />
                  <Stack.Screen options={{headerShown:false,}} name="Search" component={Search} />
                  <Stack.Screen options={{headerShown:false,}} name="InforAccount" component={InforAccount} />
              </Stack.Navigator>
    )
}
function MyTabs() {
    return (
      <Tab.Navigator tabBarOptions={{}} initialRouteName="Home">
        <Tab.Screen options={{tabBarIcon:()=>(<Icon name='home' size={30} color='#333' />)}} name="Trang Chủ" component={HomeScreen} />
        <Tab.Screen options={{tabBarIcon:()=>(<Icon name='star' size={30} color='#333' />)}} name="Danh Mục " component={CategoryPage} />
        <Tab.Screen options={{tabBarIcon:()=>(<Icon name='bars' size={30} color='#333'/>)}} name="Danh Sách Khóa Học" component={ListCoursesPage} />
        {/* <Tab.Screen options={{tabBarIcon:()=>(<Icon name='search1' size={30} color='#333'/>)}} name="Search" component={Search} /> */}
        <Tab.Screen options={{tabBarIcon:()=>(<Icon name='user' size={30} color='#333'/>)}} name="Thông Tin Tài Khoản" component={account} />
       
      </Tab.Navigator>
    );
  }
function main () {
        return (
            <NavigationContainer>
              <MyTabs/>
            </NavigationContainer>
        )
    }

const mapStateToProps = (state) =>{
  return {};
}
export default connect(mapStateToProps)(main);