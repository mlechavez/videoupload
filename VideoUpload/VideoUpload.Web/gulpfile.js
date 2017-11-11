/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    bower = require('gulp-bower'),
    sass = require('gulp-sass'),
    uglify = require('gulp-uglify'),
    notify = require('gulp-notify'),
    concat = require('gulp-concat'),
    plugins = require('gulp-load-plugins')(),
    gulpIf = require('gulp-if'),
    runSequence = require('run-sequence');

var paths = {
    vendors: './bower_components',
    sass: './assets/sass',
    js: './assets/js',
    fonts: './assets/fonts',
    images: './assets/images'
};

gulp.task('bower', function () {
    return bower().pipe(gulp.dest(paths.vendors));
});

gulp.task('sass', function () {
    return gulp.src(paths.sass + '/**/*.scss')
        .pipe(sass({
            outputStyle: 'compressed',
            includePaths: [
                paths.sass,
                paths.vendors + '/bootstrap-sass/assets/stylesheets',
                paths.vendors + '/font-awesome/scss',
                paths.vendors + '/tinymce-dist/skins/pcd/'
            ]
        })).on('error', notify.onError(function (error) {
            return 'Error: ' + error.message;
        })).pipe(concat('app.min.css')).pipe(gulp.dest('./dist/css'))
        .pipe(plugins.livereload());

    //gulp.src('assets/sass/**/*.scss')
    //    .pipe(plugins.sourcemaps.init())
    //    .pipe(plugins.sass({

    //    }))
    //    .pipe(plugins.autoprefixer())
    //    .pipe(plugins.sourcemaps.write())
    //    .pipe(gulp.dest('assets/css'))
    //    .pipe(plugins.livereload());
});

gulp.task('lib', function () {
    return gulp.src([
        paths.vendors + '/jquery/dist/jquery.min.js',
        paths.vendors + '/bootstrap-sass/assets/javascripts/bootstrap.min.js',
        paths.vendors + '/jquery.cookie/jquery.cookie.js',        
    ]).pipe(uglify()).pipe(concat('lib.min.js')).pipe(gulp.dest('./dist/js'));
});

gulp.task('js', function () {
    return gulp.src(paths.js + '/**/*.js')
        .pipe(uglify())
        .on('error', notify.onError(function (error) {
            return 'Error: ' + error.message;
        })).pipe(gulp.dest('./dist/js'));
});

gulp.task('watch', function () {
    plugins.livereload.listen();
    gulp.watch(paths.sass + '/**/*.scss', ['sass']);
    gulp.watch(paths.js + '/**/*.js', ['js']);
});

gulp.task('fonts', function () {
    return gulp.src([
        paths.vendors + '/font-awesome/fonts/**/*',
        paths.fonts + '/**/*'
    ]).pipe(gulp.dest('./dist/fonts'));
});

gulp.task('images', function () {
    return gulp.src(paths.vendors + '/**/*.+(png|jpg|jpeg|gif|svg')
        .pipe(gulp.dest('dist/images'));
});

gulp.task('default', function (callBack) {
    runSequence(['bower','sass', 'lib', 'js', 'images', 'fonts', 'watch'], callBack);
});