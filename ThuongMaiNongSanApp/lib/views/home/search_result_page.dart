import 'dart:async';

import 'package:apptmns/controllers/search_product_controller.dart';
import 'package:apptmns/core/components/network_image.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../core/components/app_back_button.dart';
import '../../core/components/product_tile_square.dart';
import '../../core/constants/constants.dart';

class SearchResultPage extends StatefulWidget {
  const SearchResultPage({super.key});

  @override
  State<SearchResultPage> createState() => _SearchResultPageState();
}

class _SearchResultPageState extends State<SearchResultPage> {

  late final SearchItemsController searchItemsController;
  bool _isLoadingMore = false;
  bool _canLoadMore = true;
  final scrollController = ScrollController();

  void _scrollListener() async {
    if(_isLoadingMore || !_canLoadMore) return;
    if(scrollController.position.pixels == scrollController.position.maxScrollExtent){
      setState(() {
        _isLoadingMore = true;
      });
      searchItemsController.currentPage.value = searchItemsController.currentPage.value + 1;
      bool canLoadMore = await searchItemsController.isGetMoreListProductsPaginated(pageSize: 6,pageIndex: searchItemsController.currentPage.value);
      if(!canLoadMore){
        searchItemsController.currentPage.value = 1;
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
    searchItemsController = Get.find<SearchItemsController>();
    searchItemsController.isGetMoreListProductsPaginated(pageSize: 6, pageIndex: 1);
    Timer(const Duration(milliseconds: 1100), () {});
    scrollController.addListener(_scrollListener);
  }

  @override
  void dispose() {
    // TODO: implement dispose
    scrollController.dispose();
    searchItemsController.clearListProductBySearch();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {

    return Scaffold(
      appBar: AppBar(
        title: const Text('Kết quả tìm kiếm'),
        leading: const AppBackButton(),
      ),
      body: Column(
        children: [
          /*Padding(
            padding: const EdgeInsets.all(AppDefaults.padding),
            child: TextField(
              decoration: InputDecoration(
                labelText: 'Search Field',
                suffixIcon: Padding(
                  padding: const EdgeInsets.all(AppDefaults.padding),
                  child: SvgPicture.asset(AppIcons.search),
                ),
                suffixIconConstraints: const BoxConstraints(),
              ),
            ),
          ),*/
          Align(
            alignment: Alignment.centerLeft,
            child: Padding(
              padding:
                  const EdgeInsets.symmetric(vertical: AppDefaults.padding, horizontal: AppDefaults.padding),
              child: Text(
                'Kết quả tìm kiếm cho từ khóa "${searchItemsController.keyword}"',
                style: Theme.of(context)
                    .textTheme
                    .bodyLarge
                    ?.copyWith(color: Colors.black),
              ),
            ),
          ),
          Expanded(
            child: Obx(() {
              if (searchItemsController.listProductBySearch.isEmpty) {
                return Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    const Spacer(flex: 2),
                    SizedBox(
                      width: MediaQuery.of(context).size.width * 0.7,
                      child: const Padding(
                        padding: EdgeInsets.all(AppDefaults.padding * 2),
                        child: AspectRatio(
                          aspectRatio: 1 / 1,
                          child:  NetworkImageWithLoader('https://i.imgur.com/mbjap7k.png'),
                        ),
                      ),
                    ),
                    Text(
                      'Oppss!',
                      style: Theme.of(context)
                          .textTheme
                          .titleLarge
                          ?.copyWith(fontWeight: FontWeight.bold, color: Colors.black),
                    ),
                    const SizedBox(height: 8),
                    const Text('Không có kết quả nào trùng khớp'),
                    const Spacer(),
                  ],
                ); // Hoặc một widget khác nếu danh sách trống
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
                      itemCount: searchItemsController.listProductBySearch.length,
                      itemBuilder: (context, index) {
                        return ProductTileSquare(
                          data: searchItemsController.listProductBySearch[index],
                          categoryName: '',
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
          ),
        ],
      ),
    );
  }
}
