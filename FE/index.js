window.getDataApi = async function () {
    const res = await axios.get('http://localhost:5124/api/Product/GetAllProducts');


    const data = res.data;

    console.log(data);


}

// add product
window.addProduct = async function (name, price, alias) {
    const product = {
        id: 0,
        name: 'loitph',
        price: 1000,
        alias: 'loitph-alias'
    };

    const res = await axios.post('http://localhost:5124/api/Product/CreateProduct', product, {
        headers: { 'Content-Type': 'application/json' }
    });

    console.log(res.data);
    return res.data;
}
