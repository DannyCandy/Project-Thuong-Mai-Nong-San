import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../controllers/product_controller.dart';
import '../../core/components/app_back_button.dart';
import '../../core/components/product_tile_square.dart';
import '../../core/constants/constants.dart';

class CategoryProductPage extends StatefulWidget {
  const CategoryProductPage({super.key, required this.categoryId, required this.categoryName});

  final String categoryId;
  final String categoryName;

  @override
  State<CategoryProductPage> createState() => _CategoryProductPageState();
}

class _CategoryProductPageState extends State<CategoryProductPage> {

  late final ProductController productController;
  bool _isLoadingMore = false;
  bool _canLoadMore = true;
  final scrollController = ScrollController();

  void _scrollListener() async {
    if(_isLoadingMore || !_canLoadMore) return;
    if(scrollController.position.pixels == scrollController.position.maxScrollExtent){
      setState(() {
        _isLoadingMore = true;
      });
      productController.currentPage.value = productController.currentPage.value + 1;
      bool canLoadMore = await productController.isGetMoreListProductsPaginated(pageSize: 6,pageIndex: productController.currentPage.value, category: widget.categoryId);
      if(!canLoadMore){
        productController.currentPage.value = 1;
      }
      setState(() {
        _isLoadingMore = false;
        _canLoadMore = canLoadMore;
      });
    }
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    productController = Get.find<ProductController>();
    productController.isGetMoreListProductsPaginated(pageSize: 6, pageIndex: 1,category: widget.categoryId);
    scrollController.addListener(_scrollListener);
  }

  @override
  void dispose() {
    // TODO: implement dispose
    scrollController.dispose();
    productController.clearListProductsTemp();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.categoryName),
        leading: const AppBackButton(),
      ),
      body: Obx(() {
        if (productController.listProductModels.isEmpty) {
          return const Center(child: CircularProgressIndicator()); // Hoặc một widget khác nếu danh sách trống
        }
        return Stack(
          children: [
            GridView.builder(
              controller: scrollController,
              padding: const EdgeInsets.only(top: AppDefaults.padding),
              gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                crossAxisCount: 2,
                mainAxisSpacing: 16,
                childAspectRatio: 0.85,
              ),
              itemCount: productController.listProductModels.length,
              itemBuilder: (context, index) {
              return ProductTileSquare(
                data: productController.listProductModels[index],
                categoryName: widget.categoryName,
              );
            },
          ),
            if (_isLoadingMore)
              const Positioned(
                bottom: 16,
                left: 0,
                right: 0,
                child: Center(child: CircularProgressIndicator()),
              ),
          ]
        );
      })
    );
  }
}
