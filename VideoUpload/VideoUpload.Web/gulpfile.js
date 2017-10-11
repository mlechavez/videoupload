/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    plugins = require('gulp-load-plugins')(),
    gulpIf = require('gulp-if'),
    runSequence = require('run-sequence');

gulp.task('sass', function () {
    gulp.src('assets/sass/**/*.scss')
        .pipe(plugins.sourcemaps.init())
        .pipe(plugins.sass())
        .pipe(plugins.autoprefixer())
        .pipe(plugins.sourcemaps.write())
        .pipe(gulp.dest('assets/css'))
        .pipe(plugins.livereload());
});

gulp.task('watch', function () {
    plugins.livereload.listen();
    gulp.watch('assets/sass/**/*.scss', ['sass']);
});

gulp.task('images', function () {
    return gulp.src('assets/images/**/*.+(png|jpg|jpeg|gif|svg')
        .pipe(gulp.dest('dist/images'));
});

gulp.task('fonts', function () {
    return gulp.src('assets/fonts/**/*')
        .pipe(gulp.dest('dist/fonts'));
});

gulp.task('build', function (callBack) {
    runSequence(['sass', 'images', 'fonts', 'css', 'js' ], callBack);
});

gulp.task('css', function () {
    return gulp.src('assets/css/**/*.css')
        .pipe(gulp.dest('dist/css'));
});

gulp.task('js', function () {
    return gulp.src('assets/js/**/*.js')
        .pipe(gulp.dest('dist/js'));
});

gulp.task('default', function (callBack) {
    runSequence(['sass', 'watch', 'build'], callBack);
});

gulp.task('minifyFilesForRelease', function () {
    var cssFilter = plugins.filter('**/*.css', { restore: true });
    var jsFilter = plugins.filter('**/*.js', { restore: true });

    gulp.src('./**/*.cshtml')


        .pipe(jsFilter)
        //minifies the javascript
        .pipe(gulpIf('*.js',plugins.uglify()))
        .pipe(plugins.rev())
        .pipe(jsFilter.restore)

        .pipe(cssFilter)
        //minifies the css
        .pipe(gulpIf('*.css', plugins.cssnano()))
        .pipe(plugins.rev())
        .pipe(cssFilter.restore)

        .pipe(plugins.useref())
        //.pipe(plugins.revReplace({
        //    replaceInExtensions: ['.js', '.css', '.html', '.cshtml']
        //}))
        .pipe(gulp.dest(function (data) {
            return data.base;
        }));
});