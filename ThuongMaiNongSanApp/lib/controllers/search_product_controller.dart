import 'dart:async';
import 'package:apptmns/logic_constants/api_endpoints.dart';
import 'package:apptmns/models/product_model.dart';
import 'package:apptmns/services/dio_db_connect.dart';
import 'package:apptmns/views/auth/dialogs/no_verified_dialogs.dart';
import 'package:get/get.dart';
import 'package:dio/dio.dart';

class SearchItemsController extends GetxController {
  RxString keyword = ''.obs;
  RxList<String> searchResults = <String>[].obs; // Kết quả tìm kiếm
  RxList<Product> listProductBySearch = <Product>[].obs;
  RxBool isLoading = false.obs; // Trạng thái loading
  RxInt currentPage = 1.obs;

  Timer? _debounce; // Biến debounce
  CancelToken? _cancelToken; // Token để hủy request Dio



  void setKeyword(String token){
    keyword.value = token;
  }

  void clearListProductBySearch(){
    listProductBySearch.clear();
  }

  // Hàm gọi API tìm kiếm
  Future<List<String>> _fetchSearchResults(String query) async {
    try {
      // Tạo CancelToken mới cho mỗi request
      _cancelToken = CancelToken();
      final DioClient dioClient = DioClient();
      // Gửi request đến API
      final response = await dioClient.dio.get(
        ApiEndpoints.searchSuggestions,
        queryParameters: {'query': query},
        cancelToken: _cancelToken, // Truyền CancelToken vào request
      );

      if(response.statusCode == 200){
        // Parse dữ liệu kết quả
        return (response.data as List<dynamic>).map((item) => item.toString()).toList();
      }
      return [];

    } on DioException catch (dioEx) {
      return [];
    }catch (e) {
      return [];
    }
  }

  // Hàm debounce tìm kiếm
  Future<void> onSearchChanged(String query) async {
    // Hủy debounce trước đó nếu đang chạy
    if (_debounce?.isActive ?? false) _debounce!.cancel();
    // Đặt debounce
    _debounce = Timer(const Duration(seconds: 1), () async {
      if (query.isNotEmpty) {
        // Hủy request trước đó nếu chưa hoàn thành
        if (_cancelToken != null && !_cancelToken!.isCancelled) {
          _cancelToken!.cancel();
        }

        isLoading.value = true; // Bắt đầu trạng thái loading

        // Gọi API tìm kiếm
        final results = await _fetchSearchResults(query);
        searchResults.value = results;
        isLoading.value = false;
      } else {
        searchResults.clear(); // Xóa kết quả nếu không có từ khóa
      }
    });
  }

  Future<bool> isGetMoreListProductsPaginated({required int pageSize, required int pageIndex, String category = "",
    String orderBy = "",
    String searchName = "",}) async {
    /*final http = ApiEndpoints.makeNoSSLSecureRequest();
    var result = await http.post(
      Uri.parse("${ApiEndpoints.getProductByPaginated}"
          "?pageSize=$pageSize&pageIndex=$pageIndex&category=$category&orderBy=$orderBy&searchName=$searchName"),
      headers: {
        'Content-Type': 'application/json; charset=UTF-8',
      },
    );*/

    try{
      final DioClient dioClient = DioClient();
      var result = await dioClient.dio.get(ApiEndpoints.getProductByPaginated,
          options: Options(
            headers: {
              'Content-Type': 'application/json; charset=UTF-8',
            },
          ),
          queryParameters: {
            'pageSize':pageSize,
            'pageIndex':pageIndex,
            'category':category,
            'orderBy':orderBy,
            'searchName':keyword
          }
      );

      if(result.statusCode == 200){

        final List<Map<String, dynamic>> tempData = (result.data as List).cast<Map<String, dynamic>>();

        if(tempData.isEmpty){
          return false;
        }

        listProductBySearch.addAll(
          tempData.map((json) => Product.fromJson(json)).toList(),
        );
        return true;
      }
      return false;
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
      return false;
    }catch(e){
      return false;
    }
  }

  @override
  void onClose() {
    // Hủy debounce và request khi controller bị đóng
    _debounce?.cancel();
    if (_cancelToken != null && !_cancelToken!.isCancelled) {
      _cancelToken!.cancel();
    }
    super.onClose();
  }
}
