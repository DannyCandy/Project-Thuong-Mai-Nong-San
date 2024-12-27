import 'package:apptmns/controllers/order_history_controller.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../../core/enums/dummy_order_status.dart';
import '../order_details.dart';
import 'order_preview_tile.dart';

class AllTab extends StatefulWidget {
  const AllTab({
    super.key,
  });
  @override
  State<AllTab> createState() => _AllTabPageState();
}

class _AllTabPageState extends State<AllTab> {

  late final OrderHistoryController orderHistoryController;
  bool _isLoadingMore = false;
  bool _canLoadMore = true;
  final scrollController = ScrollController();

  void _scrollListener() async {
    if(_isLoadingMore || !_canLoadMore) return;
    if(scrollController.position.pixels == scrollController.position.maxScrollExtent){
      setState(() {
        _isLoadingMore = true;
      });
      orderHistoryController.currentPage.value = orderHistoryController.currentPage.value + 1;
      bool canLoadMore = await orderHistoryController.canGetMoreOrderHistory(pageIndex: orderHistoryController.currentPage.value);
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
    orderHistoryController = Get.find<OrderHistoryController>();
    orderHistoryController.canGetMoreOrderHistory(pageIndex: 1);
    scrollController.addListener(_scrollListener);
  }

  @override
  void dispose() {
    // TODO: implement dispose
    scrollController.dispose();
    orderHistoryController.clearListOrderHistory();
    super.dispose();
  }
  
  @override
  Widget build(BuildContext context) {
    return Obx(() => ListView.builder(
      controller: scrollController,
      padding: const EdgeInsets.only(top: 8),
      itemCount: _isLoadingMore ? orderHistoryController.listOrderHistory.length+1 : orderHistoryController.listOrderHistory.length,
      itemBuilder: (context, index) {
        int length = orderHistoryController.listOrderHistory.length;
        if(index < length){
          final String orderId = orderHistoryController.listOrderHistory[index].orderId!;
          final String date = orderHistoryController.listOrderHistory[index].extractDate();
          final bool isSuccess = orderHistoryController.listOrderHistory[index].success!;
          return OrderPreviewTile(
            orderID: orderId,
            date: date,
            status: isSuccess ? OrderStatus.delivery : OrderStatus.confirmed,
            /*onTap: () => Navigator.push(context, MaterialPageRoute(builder: (_) => OrderDetailsPage(orderId: orderId))),*/
            onTap: () {
              Get.to(() => OrderDetailsPage(orderId: orderId));
            },
          );
        }else{
          return const Center(child: Text('Hiện không có lịch sử giao dịch.'));
        }
      },
    ));
  }
}
