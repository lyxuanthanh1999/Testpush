const categoryReducer = (state = [], action) => {
    let newCategories = null;
    switch (action.type) {
      case 'FETCH_CATEGORIES':{
        return action.categories;
      }
      default:
        return state;
    }
  };
  
  export default categoryReducer;