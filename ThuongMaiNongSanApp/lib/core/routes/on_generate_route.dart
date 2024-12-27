import 'package:apptmns/views/profile/address/update_address_page.dart';
import 'package:apptmns/views/profile/profile_view_page.dart';
import 'package:apptmns/views/profile/settings/change_avatar_page.dart';
import 'package:apptmns/views/save/save_page_profile.dart';
import 'package:flutter/Material.dart';

import '../../views/auth/forget_password_page.dart';
import '../../views/auth/intro_login_page.dart';
import '../../views/auth/login_or_signup_page.dart';
import '../../views/auth/login_page.dart';
import '../../views/auth/number_verification_page.dart';
import '../../views/auth/password_reset_page.dart';
import '../../views/auth/sign_up_page.dart';
import '../../views/cart/cart_page.dart';
import '../../views/cart/checkout_page.dart';
import '../../views/drawer/about_us_page.dart';
import '../../views/drawer/contact_us_page.dart';
import '../../views/drawer/drawer_page.dart';
import '../../views/drawer/faq_page.dart';
import '../../views/drawer/help_page.dart';
import '../../views/drawer/terms_and_conditions_page.dart';
import '../../views/entrypoint/entrypoint_ui.dart';
import '../../views/home/bundle_create_page.dart';
import '../../views/home/bundle_details_page.dart';
import '../../views/home/bundle_product_details_page.dart';
import '../../views/home/new_item_page.dart';
import '../../views/home/order_failed_page.dart';
import '../../views/home/order_successfull_page.dart';
import '../../views/home/popular_pack_page.dart';
import '../../views/home/product_details_page.dart';
import '../../views/home/search_page.dart';
import '../../views/home/search_result_page.dart';
import '../../views/menu/category_page.dart';
import '../../views/onboarding/onboarding_page.dart';
import '../../views/profile/address/address_page.dart';
import '../../views/profile/address/new_address_page.dart';
import '../../views/profile/coupon/coupon_details_page.dart';
import '../../views/profile/coupon/coupon_page.dart';
import '../../views/profile/notification_page.dart';
import '../../views/profile/order/my_order_page.dart';
import '../../views/profile/order/order_details.dart';
import '../../views/profile/payment_method/add_new_card_page.dart';
import '../../views/profile/payment_method/payment_method_page.dart';
import '../../views/profile/profile_edit_page.dart';
import '../../views/profile/settings/change_password_page.dart';
import '../../views/profile/settings/change_email_page.dart';
import '../../views/profile/settings/language_settings_page.dart';
import '../../views/profile/settings/notifications_settings_page.dart';
import '../../views/profile/settings/settings_page.dart';
import '../../views/review/review_page.dart';
import '../../views/review/submit_review_page.dart';
import '../../views/save/save_page.dart';
import 'app_routes.dart';
import 'unknown_page.dart';

class RouteGenerator {
  static Route? onGenerate(RouteSettings settings) {
    final route = settings.name;

    switch (route) {
      case AppRoutes.introLogin:
        return MaterialPageRoute(builder: (_) => const IntroLoginPage());

      /*case AppRoutes.onboarding:
        return MaterialPageRoute(builder: (_) => const OnboardingPage());*/

      case AppRoutes.entryPoint:
        return MaterialPageRoute(builder: (_) => const EntryPointUI());

      case AppRoutes.search:
        return MaterialPageRoute(builder: (_) => const SearchPage());

      case AppRoutes.searchResult:
        return MaterialPageRoute(builder: (_) => const SearchResultPage());

      case AppRoutes.cartPage:
        return MaterialPageRoute(builder: (_) => const CartPage());

      case AppRoutes.savePage:
        return MaterialPageRoute(builder: (_) => const SavePage());

      case AppRoutes.savePageProfile:
        return MaterialPageRoute(builder: (_) => const SavePageProfile());

      case AppRoutes.checkoutPage:
        return MaterialPageRoute(builder: (_) => const CheckoutPage());

      /*case AppRoutes.categoryDetails:
        return MaterialPageRoute(builder: (_) => const CategoryProductPage());*/

      case AppRoutes.login:
        return MaterialPageRoute(builder: (_) => const LoginPage());

      case AppRoutes.signup:
        return MaterialPageRoute(builder: (_) => const SignUpPage());

      /*case AppRoutes.loginOrSignup:
        return MaterialPageRoute(builder: (_) => const LoginOrSignUpPage());*/

      /*case AppRoutes.numberVerification:
        return MaterialPageRoute(
            builder: (_) => const NumberVerificationPage(userEmail: 'dankbzzz01.2@gmail.com',));*/

      case AppRoutes.forgotPassword:
        return MaterialPageRoute(builder: (_) => const ForgetPasswordPage());

      case AppRoutes.passwordReset:
        return MaterialPageRoute(builder: (_) => const PasswordResetPage());

      case AppRoutes.newItems:
        return MaterialPageRoute(builder: (_) => const NewItemsPage());

      /*case AppRoutes.popularItems:
        return MaterialPageRoute(builder: (_) => const PopularPackPage());*/

      /*case AppRoutes.bundleProduct:
        return MaterialPageRoute(
            builder: (_) => const BundleProductDetailsPage());*/

      /*case AppRoutes.bundleDetailsPage:
        return MaterialPageRoute(builder: (_) => const BundleDetailsPage());*/

      /*case AppRoutes.productDetails:
        return MaterialPageRoute(builder: (_) => const ProductDetailsPage());*/

      /*case AppRoutes.createMyPack:
        return MaterialPageRoute(builder: (_) => const BundleCreatePage());*/

      case AppRoutes.orderSuccessfull:
        return MaterialPageRoute(builder: (_) => const OrderSuccessfullPage());

      case AppRoutes.orderFailed:
        return MaterialPageRoute(builder: (_) => const OrderFailedPage());

      case AppRoutes.myOrder:
        return MaterialPageRoute(builder: (_) => const AllOrderPage());

      /*case AppRoutes.orderDetails:
        return MaterialPageRoute(builder: (_) => const OrderDetailsPage());*/

      /*case AppRoutes.coupon:
        return MaterialPageRoute(builder: (_) => const CouponAndOffersPage());*/

      /*case AppRoutes.couponDetails:
        return MaterialPageRoute(builder: (_) => const CouponDetailsPage());*/

      case AppRoutes.profileEdit:
        return MaterialPageRoute(builder: (_) => const ProfileEditPage());

      case AppRoutes.newAddress:
        return MaterialPageRoute(builder: (_) => const NewAddressPage());

      case AppRoutes.deliveryAddress:
        return MaterialPageRoute(builder: (_) => const AddressPage());

      /*case AppRoutes.notifications:
        return MaterialPageRoute(builder: (_) => const NotificationPage());*/

      /*case AppRoutes.settingsNotifications:
        return MaterialPageRoute(
            builder: (_) => const NotificationSettingsPage());*/

      case AppRoutes.settings:
        return MaterialPageRoute(builder: (_) => const SettingsPage());

      /*case AppRoutes.settingsLanguage:
        return MaterialPageRoute(builder: (_) => const LanguageSettingsPage());*/

      /*case AppRoutes.review:
        return MaterialPageRoute(builder: (_) => const ReviewPage());

      case AppRoutes.submitReview:
        return MaterialPageRoute(builder: (_) => const SubmitReviewPage());*/

      case AppRoutes.drawerPage:
        return MaterialPageRoute(builder: (_) => const DrawerPage());

      case AppRoutes.aboutUs:
        return MaterialPageRoute(builder: (_) => const AboutUsPage());

      case AppRoutes.termsAndConditions:
        return MaterialPageRoute(
            builder: (_) => const TermsAndConditionsPage());

      case AppRoutes.faq:
        return MaterialPageRoute(builder: (_) => const FAQPage());

      /*case AppRoutes.help:
        return MaterialPageRoute(builder: (_) => const HelpPage());*/

      case AppRoutes.contactUs:
        return MaterialPageRoute(builder: (_) => const ContactUsPage());

      /*case AppRoutes.paymentMethod:
        return MaterialPageRoute(builder: (_) => const PaymentMethodPage());

      case AppRoutes.paymentCardAdd:
        return MaterialPageRoute(builder: (_) => const AddNewCardPage());*/

      case AppRoutes.changeAvatar:
        return MaterialPageRoute(builder: (_) => const AvatarEditPage());

      case AppRoutes.profileViewer:
        return MaterialPageRoute(builder: (_) => const ProfileViewPage());

      case AppRoutes.changePassword:
        return MaterialPageRoute(builder: (_) => const ChangePasswordPage());

      case AppRoutes.changeEmail:
        return MaterialPageRoute(builder: (_) => const ChangeEmailPage());

      /*case AppRoutes.updateDeliveryAddress:
        return MaterialPageRoute(builder: (_) => const UpdateAddressPage());*/

      default:
        return errorRoute();
    }
  }

  static Route? errorRoute() =>
      MaterialPageRoute(builder: (_) => const UnknownPage());
}
