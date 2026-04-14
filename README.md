===== HƯỚNG DẪN LÀM VIỆC NHÓM =====

1. Clone project từ Git về máy:
   git clone <link repo>

2. Không code trực tiếp trên nhánh main

3. Mỗi người tạo branch riêng:
   git checkout -b feature-tenchucnang

   Ví dụ:
   feature-dichvu
   feature-khachhang
   feature-hoadon

4. Sau khi làm xong:
   git add .
   git commit -m "mo ta chuc nang"
   git push origin ten-branch

5. Báo trưởng nhóm merge vào main

6. Trước khi code phải chạy:
   git pull origin main

7. Không tự ý sửa database nếu chưa thống nhất


====== PHÂN CÔNG CÔNG VIỆC ======
Phân công như sau:

* Ngọc Anh: Đặt phòng + Hóa đơn
* Hoa: Loại phòng + Thống kê
* Ngọc: Đăng nhập + Phòng
* Nga: Danh sách đặt phòng + Nhân viên
* Minh: Dịch vụ + Khách hàng

Mỗi người làm đúng phần mình, không sửa lung tung file người khác.
