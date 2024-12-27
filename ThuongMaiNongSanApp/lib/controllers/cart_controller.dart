
import 'package:apptmns/core/routes/app_routes.dart';
import 'package:apptmns/logic_constants/api_endpoints.dart';
import 'package:apptmns/models/order_request_model.dart';
import 'package:apptmns/services/dio_db_connect.dart';
import 'package:apptmns/services/user_info_store.dart';
import 'package:apptmns/views/auth/dialogs/info_dialogs.dart';
import 'package:apptmns/views/profile/address/address_page.dart';
import 'package:dio/dio.dart';
import 'package:flutter/Material.dart';
import 'package:get/get.dart';

import '../views/auth/dialogs/no_verified_dialogs.dart';

class CartController extends GetxController {
  RxList<Map<String, dynamic>> listCartItem = <Map<String, dynamic>>[].obs;
  RxInt quantityOfCurrentItem = 1.obs;
  RxBool isLoading = false.obs;

  final Rx<TextEditingController> textFormField = TextEditingController().obs;

  void addToCart(String productId, String productName, String productImg, int quantity, int price){
    bool isExist = listCartItem.any((item) => item['productId'] == productId);
    if(isExist){
      int index = getIndexOfSingleItemFromCart(productId);
      listCartItem[index]['quantity'] += quantity;
      listCartItem[index]['totalPrice'] = listCartItem[index]['price']*listCartItem[index]['quantity'];
    }else{
      Map<String, dynamic> cartItem = {
        'productId': productId,
        'productName':productName,
        'productImage':productImg,
        'quantity':quantity,
        'price':price,
        'totalPrice':price*quantity
      };
      listCartItem.add(cartItem);
    }
    quantityOfCurrentItem.value = quantity;
  }

  void removeFromCart(String productIdToDel){
    if(listCartItem.length < 2){
      Get.dialog(const NoVerifiedDialogs(message: "Không thể xóa vì đây là sản phẩm duy nhất trong giỏ hàng"));
      return;
    }
    listCartItem.removeWhere((item) => item['productId'] == productIdToDel);
  }

  void clearCart(){
    listCartItem.clear();
  }

  int getNumOfItemInCart(){
    return listCartItem.length;
  }

  int getTotalPriceOfCart(){
    int total = 0;
    for (var item in listCartItem) {
      total += item['totalPrice'] as int;
    }
    return total;
  }

  void setQuantityOfCurrentItem(int quantity){
    quantityOfCurrentItem.value = quantity;
  }

  int getQuantityOfCurrentItem(){
    return quantityOfCurrentItem.value;
  }

  int getIndexOfSingleItemFromCart(String id){
    return listCartItem.indexWhere((i) => i['productId'] == id);
  }

  void setQuantityAndTotalPriceOfItem(String id, int quantity){
    if(quantity == 0) return;
    int index = listCartItem.indexWhere((item) => item['productId'] == id);
    if(index == -1) return;
    listCartItem[index]['quantity'] = quantity;
    listCartItem[index]['totalPrice'] = quantity * listCartItem[index]['price'];
    listCartItem.refresh();
  }

  //Order
  Future<void> checkOut(String addressId) async {
    isLoading.value = true;
    try{
      if(addressId.isEmpty){
        Get.dialog(
          InfoDialog(
              onPressed: (){
                Get.to(() => const AddressPage());
              },
              message: 'Hiện chưa có địa chỉ giao hàng nào\nHãy thiết lập ngay!',
              buttonTitle: 'Đến thiết lập')
        );
        return;
      }
      final OrderRequestModel order = OrderRequestModel(
        cart: Cart(
          items: listCartItem.map((item) =>
              Items(
                name: item['productName'],
                price: item['price'],
                productId: item['productId'],
                productImg: item['productImage'],
                quantity: item['quantity'],
              )
          ).toList(),
        ),
        message: textFormField.value.text,
      );
      final DioClient dioClient = DioClient();
      var result = await dioClient.dio.post(ApiEndpoints.checkOut,
          options: Options(
              headers: {
                'Content-Type': 'application/json; charset=UTF-8',
                'Authorization': 'Bearer ${UserPreferences.getUserToken()}',
              }
          ),
          data: order.toJson(),
          queryParameters: {
            'addressId':addressId,
            'payment':'COD'
          }
      );
      if (result.statusCode == 200) {
        clearCart();
        Get.offAllNamed(AppRoutes.orderSuccessfull);
        return;
      }

    }on DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
      return;
    }catch(e){
      return;
    }finally{
      isLoading.value = false;
    }
  }
}
