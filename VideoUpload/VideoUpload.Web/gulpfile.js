/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var sass = require('gulp-sass');

gulp.task('copy-js-to-dist', function () {
  return gulp.src('bower_components/jquery/dist/**/*.js')
    .pipe(gulp.dest('dist/js'));
});

gulp.task('compile-sass', function () {
  return gulp.src('assets/sass/**/*.scss')
    .pipe(sass())
    .pipe(gulp.dest('assets/css'));
});

gulp.task('watch-sass', function () {
  gulp.watch('assets/sass/**/*.scss', ['compile-sass']);
});

gulp.task('default', function () {
    // place code for your default task here
});