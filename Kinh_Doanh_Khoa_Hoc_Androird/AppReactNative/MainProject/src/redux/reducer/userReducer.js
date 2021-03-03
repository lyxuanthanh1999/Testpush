const userReducer = (state = [], action) => {
    let newCourses = null;
    switch (action.type) {
      case 'FETCH_USER':{
        return action.user;
      }
      default:
        return state;
    }
  };
  
  export default userReducer;