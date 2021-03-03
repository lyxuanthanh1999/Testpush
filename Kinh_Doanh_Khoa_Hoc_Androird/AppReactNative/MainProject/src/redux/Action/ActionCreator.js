import actionTypes from './ActionType'
import Axios from 'axios';
import { Provider } from 'react-redux';
import { Alert } from 'react-native';
// localhost 192.168.1.104 192.168.0.192
const URL ='http://192.168.1.4:4000/api/';

function fetchCourses(){
    return (dispatch)=>{
        Axios.get(URL+'Courses/new-courses')
        .then((response) =>{
            const data = response.data;
                dispatch({
                    type: actionTypes.ACTION_TYPE_FETCH_COURSES,
                    courses : data,
                    loading:false,
                })
        })
    }
}
// side-bar home-categories
function fetchCategories(){
    return(dispatch)=>{
        Axios.get(URL + 'Categories/side-bar')
        .then((response)=>{
            const data = response.data;
            console.log(JSON.stringify(data))
            dispatch({
                type:actionTypes.ACTION_TYPE_FETCH_CATGORIES,
                categories : data,
            })
        })
    }
}
// http://localhost:4000/api/Courses/1
function fetchCoursesId(id){
    return (dispatch)=>{
        Axios.get(URL+'Courses/'+id)
        .then((response) =>{
            const data = response.data;
            dispatch({
                type:actionTypes.ACTION_TYPE_FETCH_COURSES_ID,
                courseID : data,
            })
        })
    }
}

// http://192.168.0.192:4000/api/Courses/filter?pageIndex=1&pageSize=65
function fetchListCourses(page){
    console.log('ActionCreator:'+page)
    return (dispatch)=>{
        Axios.get(URL+`Courses/filter?pageIndex=${page}&pageSize=5`)
        .then((response) =>{
            const data = response.data;
            console.log('data của list Courses : '+JSON.stringify(data))
                dispatch({
                    type: actionTypes.ACTION_TYPE_FETCH_COURSES_LIST,
                    listCourses : data.items,
                })
        })
    }
}


//======================= chưa dùng =================================

// http://localhost:4000/api/Users/user-user1
function fetchUser(name){
    return (dispatch) => {
        Axios.get(URL + 'Users/user-'+name)
        .then((response)=>{
            const data = response.data;
            console.log('login dc')
            dispatch({
                type:actionTypes.ACTION_TYPE_FETCH_COURSES_LIST,
                user : data,
                result : true,
            })
        })
        .catch((error)=>{
            console.log('login k dc')
            dispatch({
                type:actionTypes.ACTION_TYPE_FETCH_COURSES_LIST,
                user : error,
                result : false,
            })
        })
    }
}



// http://192.168.0.192:4000/api/Categories/side-bar
function fetchCategoriesSlideBar(){
    return(dispatch)=>{
        Axios.get(URL + 'Categories/side-bar')
        .then((response)=>{
            const data = response.data;
            dispatch({
                type:actionTypes.ACTION_TYPE_FETCH_CATGORIES_SLIDEBAR,
                categoriesSlideBar : data,
            })
        })
    }
}

export default {fetchCoursesId,fetchCourses,fetchCategories,fetchListCourses,fetchCategoriesSlideBar,fetchUser};