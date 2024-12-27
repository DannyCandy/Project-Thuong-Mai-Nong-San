import 'package:apptmns/controllers/search_product_controller.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';
import 'package:get/get.dart';

import '../../core/components/app_back_button.dart';
import '../../core/constants/app_colors.dart';
import '../../core/constants/app_defaults.dart';
import '../../core/constants/app_icons.dart';
import '../../core/routes/app_routes.dart';

class SearchPage extends StatelessWidget {
  const SearchPage({super.key});

  @override
  Widget build(BuildContext context) {
    return const Scaffold(
      body: SafeArea(
        child: Column(
          children: [
            _SearchPageHeader(),
            SizedBox(height: 8),
            _RecentSearchList(),
          ],
        ),
      ),
    );
  }
}

class _RecentSearchList extends StatelessWidget {
  const _RecentSearchList();

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => SearchItemsController());
    final searchController = Get.find<SearchItemsController>();

    return Expanded(
      child: Column(
        children: [
          /*Padding(
            padding:
                const EdgeInsets.symmetric(horizontal: AppDefaults.padding),
            child: Align(
              alignment: Alignment.centerLeft,
              child: Text(
                'Search Result',
                style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                      color: Colors.black,
                    ),
              ),
            ),
          ),*/
          Obx((){
            if(searchController.isLoading.value){
              return const Center(child: CircularProgressIndicator());
            }
            return Expanded(
              child: ListView.separated(
                padding: const EdgeInsets.only(top: 16),
                itemBuilder: (context, index) {
                  return SearchHistoryTile(
                    suggestProductName: searchController.searchResults[index],
                  );
                },
                separatorBuilder: (context, index) => const Divider(
                  thickness: 0.1,
                ),
                itemCount: searchController.searchResults.length,
              ),
            );
          })
        ],
      ),
    );
  }
}

class _SearchPageHeader extends StatefulWidget {
  const _SearchPageHeader();
  @override
  State<_SearchPageHeader> createState() => _SearchPageHeaderState();
}

class _SearchPageHeaderState extends State<_SearchPageHeader> {
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  final TextEditingController _searchName = TextEditingController();

  @override
  void dispose(){
    _searchName.dispose();
    Get.delete<SearchItemsController>();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => SearchItemsController());
    final searchController = Get.find<SearchItemsController>();

    return Padding(
      padding: const EdgeInsets.all(AppDefaults.padding),
      child: Row(
        children: [
          const AppBackButton(),
          const SizedBox(width: 16),
          Expanded(
            child: Stack(
              children: [
                /// Search Box
                Form(
                  key: _formKey,
                  child: TextFormField(
                    controller: _searchName,
                    decoration: InputDecoration(
                      hintText: 'Nhập từ khóa...',
                      prefixIcon: Padding(
                        padding: const EdgeInsets.all(AppDefaults.padding),
                        child: SvgPicture.asset(
                          AppIcons.search,
                          colorFilter: const ColorFilter.mode(
                            AppColors.primary,
                            BlendMode.srcIn,
                          ),
                        ),
                      ),
                      prefixIconConstraints: const BoxConstraints(),
                      contentPadding: EdgeInsets.zero,
                      constraints: const BoxConstraints(),
                    ),
                    textInputAction: TextInputAction.search,
                    autofocus: true,
                    onChanged: (String value) async {
                      await searchController.onSearchChanged(value);
                    },
                    onFieldSubmitted: (v) {
                      searchController.setKeyword(v);
                      Navigator.pushNamed(context, AppRoutes.searchResult);
                    },
                  ),
                ),
                Positioned(
                  right: 0,
                  height: 56,
                  child: SizedBox(
                    width: 56,
                    child: ElevatedButton(
                      onPressed: () {
                        searchController.setKeyword(_searchName.text);
                        Navigator.pushNamed(context, AppRoutes.searchResult);
                        /*UiUtil.openBottomSheet(
                          context: context,
                          widget: const ProductFiltersDialog(),
                        );*/
                      },
                      style: ElevatedButton.styleFrom(
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(8),
                        ),
                      ),
                      child: SvgPicture.asset(AppIcons.search),
                    ),
                  ),
                )
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class SearchHistoryTile extends StatelessWidget {
  const SearchHistoryTile({
    required this.suggestProductName,
    super.key,
  });

  final String suggestProductName;

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => SearchItemsController());
    final searchController = Get.find<SearchItemsController>();

    return InkWell(
      onTap: () {
        searchController.setKeyword(suggestProductName);
        Navigator.pushNamed(context, AppRoutes.searchResult);
      },
      child: Padding(
        padding: const EdgeInsets.symmetric(
          vertical: 8,
          horizontal: 16,
        ),
        child: Row(
          children: [
            Text(
              suggestProductName,
              style: Theme.of(context).textTheme.bodyMedium,
            ),
            const Spacer(),
            SvgPicture.asset(AppIcons.searchTileArrow),
          ],
        ),
      ),
    );
  }
}
