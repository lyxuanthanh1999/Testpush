
const coursesReducer = (state = [], action) => {
  let newCourses = null;
  switch (action.type) {
    case 'FETCH_COURSES':{
      return  action.courses;
    }
    case 'FETCH_COURSES_LIST':{
      return  state.concat(action.listCourses)
    }
    case 'FETCH_COURSES_ID':{
      return action.courseID;
    }
    default:
      return state;
  }
};
// const coursesReducer = (state = [], action) => {
//     let newCourses = null;
//     switch (action.type) {
//       case 'FETCH_COURSES':{
//         return action.courses;
//       }
//       case 'FETCH_COURSES_LIST':{
//         return action.listCourses
//       //   return {
//       //     ...state,
//       //     state:action.listCourses};
//        }
//        case 'FETCH_COURSES_ID':{
//           return action.CoursesId;
//        }
//       default:
//         return state;
//     }
//   };
  
  export default coursesReducer;