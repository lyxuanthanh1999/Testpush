import React, { Component } from 'react'
import { Text, View,Dimensions,FlatList, Alert } from 'react-native'
import AppDimensions from '../helpers/AppDimensions'
import ListCourseItem from '../compoments/LessonListItem'
import ListCourseItem1 from '../compoments/ListCourseItem1'
import { connect } from 'react-redux';
import ActionCreator from '../redux/Action/ActionCreator';

class ListCourses extends Component {

    constructor(props)
    {
        super(props);
        this.state = {
            page : 1,
            data:[],
            loading:false,

        }
        
    }
    componentDidMount(){
        this.props.fetchListCourses(this.state.page);
        // this.setState(data = this.props.fetchListCourses(this.state.page));
    }
    onLoadMore = () => {
        // if(!this.props.loading){
        //     this.props.fetchListCourses(this.state.page + 1);
        //     this.setState({page:page+1})
        // }
       if(this.state.loading)
       {
            console.log('trang thai loading 1 : '+this.state.loading)
           return
       }
        this.setState({loading:true},()=>{
        this.props.fetchListCourses(this.state.page + 1);
        this.setState({loading:false});
       })
    }
    render() {
        return (
            <View style={{backgroundColor:'black',height:Dimensions.get('screen').height}}>
            <View style={{alignItems:'center',justifyContent:'center'}}>
                <View style={{width:AppDimensions.getWith()}}>
                <FlatList
                    data={this.props.courses}
                    keyExtractor={(item, index) => item.id.toString()}
                    renderItem={({item, index}) => (
                    <ListCourseItem1
                        item={item}
                        navigation={this.props.navigation}
                        index={index} />
                    )}
                    onEndReachedThreshold={0.5}
                    onEndReached={this.onLoadMore}
                />
                </View>
                </View>
            </View>
        )
    }
}
const mapStateToProps = (state)=>{
    return {courses : state.courses}
}

export default connect(mapStateToProps,ActionCreator)(ListCourses);


// export default class App extends Component {
//     constructor() {
//       super();
//       this.state = {
//         loading: false,
//       };
//     }
   
//     loadMoreData = () => {
//       if (this.state.loading) {
//           return
//       }
  
//       this.setState({ loading: true }, () => { 
//         fetch('https://www.doviz.com/api/v1/currencies/all/latest')
//             .then(response => response.json())
//             .then(responseJson => {
//               this.setState({
//                 loading: false,
//               });
//             })
//             .catch(error => {
//               this.setState({
//                 loading: false,
//               });
//             });
//       });
//     };
  
//     render() {
//       return (
//             <FlatList 
//               onEndReached={this.loadMoreData}
//               onEndReachedThreshold ={0.1}
//             />
//         </View>
//       );
//     }
//   }